namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogModule.Web.Converters;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using CatalogModule = VirtoCommerce.CatalogModule.Web.Model;

    public class BulkPropertyUpdateManager : IBulkPropertyUpdateManager
    {
        private readonly IDataSourceFactory _dataSourceFactory;

        private readonly IItemService _itemService;

        private readonly Dictionary<string, MethodInfo> _productProperties = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkPropertyUpdateManager"/> class.
        /// </summary>
        /// <param name="dataSourceFactory">
        /// The data source factory.
        /// </param>
        /// <param name="itemService">
        /// The item service.
        /// </param>
        public BulkPropertyUpdateManager(IDataSourceFactory dataSourceFactory, IItemService itemService)
        {
            _dataSourceFactory = dataSourceFactory;
            _itemService = itemService;
        }

        public Property[] GetProperties(BulkActionContext context)
        {
            var result = new List<Property>();
            var propertyIds = new HashSet<string>();
            var dataSource = _dataSourceFactory.Create(context);
            result.AddRange(GetStandardProperties());

            while (dataSource.Fetch())
            {
                var productIds = dataSource.Items.Select(item => item.Id).ToArray();
                var products = _itemService.GetByIds(
                    productIds,
                    ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties);

                // using only product inherited properties from categories,
                // own product props (only from PropertyValues) are not set via bulk update action 
                var newProperties = products.SelectMany(CollectionSelector())
                    .Distinct(AnonymousComparer.Create<Property, string>(property => property.Id))
                    .Where(property => !propertyIds.Contains(property.Id)).ToArray();

                propertyIds.AddRange(newProperties.Select(property => property.Id));
                result.AddRange(newProperties);
            }

            return result.ToArray();
        }

        public BulkActionResult UpdateProperties(CatalogProduct[] products, CatalogModule.Property[] properties)
        {
            var result = new BulkActionResult { Succeeded = true };
            var hasChanges = false;

            if (products.IsNullOrEmpty())
            {
                // idle
            }
            else
            {
                hasChanges = TryChangeProductPropertyValues(properties, products, result);
            }

            if (hasChanges)
            {
                _itemService.Update(products);
            }

            return result;
        }

        private static bool AddPropertyValues(IHasProperties product, CatalogModule.Property property)
        {
            bool result;
            var defaultProperty = product.Properties.FirstOrDefault(p => p.Id.EqualsInvariant(property.Id));
            if (defaultProperty == null)
            {
                result = false;
            }
            else
            {
                if (product.PropertyValues == null)
                {
                    product.PropertyValues = new List<PropertyValue>();
                }

                foreach (var propertyValue in property.Values.Select(value => value.ToCoreModel()))
                {
                    propertyValue.Property = defaultProperty;
                    propertyValue.PropertyId = defaultProperty.Id;
                    propertyValue.PropertyName = defaultProperty.Name;
                    product.PropertyValues.Add(propertyValue);
                }

                result = true;
            }

            return result;
        }

        private static Func<CatalogProduct, IEnumerable<Property>> CollectionSelector()
        {
            return product =>
                product.Properties.Where(property => property.IsInherited && property.Type != PropertyType.Category);
        }

        private static object ConvertValue(PropertyValueType valueType, object value)
        {
            object result;

            switch (valueType)
            {
                case PropertyValueType.LongText:
                    result = Convert.ToString(value);
                    break;
                case PropertyValueType.ShortText:
                    result = Convert.ToString(value);
                    break;
                case PropertyValueType.Number:
                    try
                    {
                        result = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        result = Convert.ToString(value, CultureInfo.InstalledUICulture);
                    }

                    break;
                case PropertyValueType.DateTime:
                    result = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                    break;
                case PropertyValueType.Boolean:
                    result = Convert.ToBoolean(value);
                    break;
                case PropertyValueType.Integer:
                    result = Convert.ToInt32(value);
                    break;
                case PropertyValueType.GeoPoint:
                    result = Convert.ToString(value);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        private static bool TrySetCustomProperty(IHasProperties product, CatalogModule.Property property)
        {
            bool result;

            if (property.Multivalue || property.Dictionary)
            {
                var propertyValues = product.PropertyValues?.Where(
                    propertyValue => propertyValue.Property != null
                                     && propertyValue.Property.Id.EqualsInvariant(property.Id)).ToArray();

                if (propertyValues.IsNullOrEmpty())
                {
                    // idle
                }
                else
                {
                    if (propertyValues == null)
                    {
                        // idle
                    }
                    else
                    {
                        foreach (var productPropertyValue in propertyValues)
                        {
                            product.PropertyValues?.Remove(productPropertyValue);
                        }
                    }
                }

                result = AddPropertyValues(product, property);
            }
            else
            {
                var productPropertyValue = product.PropertyValues?.FirstOrDefault(
                    propertyValue => propertyValue.Property != null
                                     && propertyValue.Property.Id.EqualsInvariant(property.Id));

                if (productPropertyValue != null)
                {
                    var propertyValue = property.Values.FirstOrDefault();

                    productPropertyValue.Value = propertyValue?.Value;

                    result = true;
                }
                else
                {
                    result = AddPropertyValues(product, property);
                }
            }

            return result;
        }

        private MethodInfo GetProductPropertySetter(CatalogProduct product, CatalogModule.Property property)
        {
            var name = property.Name;

            if (_productProperties.TryGetValue(name, out var result))
            {
                return result;
            }

            var productType = product.GetType();
            var productProperty = productType.GetProperty(name);
            result = productProperty?.GetSetMethod();

            _productProperties.Add(name, result);

            return result;
        }

        private IList<Property> GetStandardProperties()
        {
            // TechDebt: Should get all product inherited properties faster,
            // by getting all properties for category line entries (including outline) + all inherited product line entry properties
            var result = new List<Property>
                         {
                             new Property
                             {
                                 Name = nameof(CatalogProduct.Name),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.LongText,
                                 Required = true
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.StartDate),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.DateTime,
                                 Required = true,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.EndDate),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.DateTime,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.Priority),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Integer,
                                 Required = true,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.EnableReview),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.IsActive),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.IsBuyable),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.TrackInventory),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.MinQuantity),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Integer,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.MaxQuantity),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Integer,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.Vendor),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.WeightUnit),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.Weight),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.MeasureUnit),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.PackageType),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.Height),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.Width),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.Length),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             },
                             new Property
                             {
                                 Name = nameof(CatalogProduct.TaxType),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                                 Dictionary = true
                             },
                         };

            return result;
        }

        private bool TryChangeProductPropertyValues(
            CatalogModule.Property[] properties,
            IEnumerable<CatalogProduct> products,
            BulkActionResult result)
        {
            var hasChanges = false;

            foreach (var product in products)
            {
                try
                {
                    foreach (var property in properties)
                    {
                        if (string.IsNullOrEmpty(property.Id))
                        {
                            if (string.IsNullOrEmpty(property.Name))
                            {
                                continue;
                            }

                            hasChanges = TrySetOwnProperty(product, property) || hasChanges;
                        }
                        else
                        {
                            hasChanges = TrySetCustomProperty(product, property) || hasChanges;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Succeeded = false;
                    result.Errors.Add(e.Message);
                }
            }

            return hasChanges;
        }

        private bool TrySetOwnProperty(CatalogProduct product, CatalogModule.Property property)
        {
            bool result;
            var propertyValue = property.Values.FirstOrDefault();
            var value = (property.Dictionary && !string.IsNullOrEmpty(propertyValue?.ValueId))
                            ? propertyValue.ValueId
                            : propertyValue?.Value;

            var setter = GetProductPropertySetter(product, property);

            if (setter == null)
            {
                result = false;
            }
            else
            {
                if (value == null && property.Required)
                {
                    var message = $"Property value is missing for required property \"{property.Name}\".";
                    throw new ArgumentException(message);
                }

                var convertedValue = value != null ? ConvertValue(property.ValueType, value) : null;

                setter.Invoke(product, new[] { convertedValue });
                result = true;
            }

            return result;
        }
    }
}