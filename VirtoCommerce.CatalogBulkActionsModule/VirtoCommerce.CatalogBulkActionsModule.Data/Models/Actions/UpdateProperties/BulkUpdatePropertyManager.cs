namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using moduleCoreModels = Core.Models;

    public class BulkUpdatePropertyManager : IBulkUpdatePropertyManager
    {
        private readonly IPagedDataSourceFactory _dataSourceFactory;

        private readonly IItemService _itemService;

        private readonly Dictionary<string, MethodInfo> _productProperties = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdatePropertyManager"/> class.
        /// </summary>
        /// <param name="dataSourceFactory">
        /// The data source factory.
        /// </param>
        /// <param name="itemService">
        /// The item service.
        /// </param>
        public BulkUpdatePropertyManager(IPagedDataSourceFactory dataSourceFactory, IItemService itemService)
        {
            _dataSourceFactory = dataSourceFactory;
            _itemService = itemService;
        }

        public virtual Property[] GetProperties(UpdatePropertiesActionContext context)
        {
            // TechDebt: Should get all product inherited properties faster,
            // by getting all properties for category line entries (including outline) + all inherited product line entry properties
            var dataSource = _dataSourceFactory.Create(context);
            var result = new List<Property>();
            var propertyIds = new HashSet<string>();

            result.AddRange(GetStandardProperties());

            while (dataSource.Fetch())
            {
                var productIds = dataSource.Items.Select(item => item.Id).ToArray();
                var products = _itemService.GetByIds(
                    productIds,
                    ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties);

                // using only product inherited properties from categories,
                // own product props (only from PropertyValues) are not set via bulk update 
                var newProperties = products.SelectMany(product => product.Properties.Where(property => property.IsInherited))
                    .Distinct(AnonymousComparer.Create<Property, string>(property => property.Id))
                    .Where(property => !propertyIds.Contains(property.Id)).ToArray();

                propertyIds.AddRange(newProperties.Select(property => property.Id));
                result.AddRange(newProperties);
            }

            return result.ToArray();
        }

        public virtual UpdatePropertiesResult UpdateProperties(
            CatalogProduct[] products,
            moduleCoreModels.Property[] propertiesToSet)
        {
            var result = new UpdatePropertiesResult { Succeeded = true };
            var hasChanges = false;

            if (products.IsNullOrEmpty())
            {
                // idle
            }
            else
            {
                hasChanges = ChangesProductPropertyValues(propertiesToSet, products, result);
            }

            if (hasChanges)
            {
                _itemService.Update(products);
            }

            return result;
        }

        protected virtual bool ChangesProductPropertyValues(
            moduleCoreModels.Property[] propertiesToSet,
            CatalogProduct[] products,
            UpdatePropertiesResult updateResult)
        {
            var hasChanges = false;

            foreach (var product in products)
            {
                try
                {
                    foreach (var propertyToSet in propertiesToSet)
                    {
                        if (string.IsNullOrEmpty(propertyToSet.Id))
                        {
                            if (string.IsNullOrEmpty(propertyToSet.Name))
                            {
                                continue;
                            }

                            hasChanges = SetOwnProperty(product, propertyToSet) || hasChanges;
                        }
                        else
                        {
                            hasChanges = SetCustomProperty(product, propertyToSet) || hasChanges;
                        }
                    }
                }
                catch (Exception e)
                {
                    updateResult.Succeeded = false;
                    updateResult.Errors.Add(e.Message);
                }
            }

            return hasChanges;
        }

        protected virtual object ConvertValue(PropertyValueType valueType, object value)
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
                    result = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
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

        protected virtual MethodInfo GetProductPropertySetter(CatalogProduct product, moduleCoreModels.Property propertyToSet)
        {
            var propertyName = propertyToSet.Name;

            if (_productProperties.TryGetValue(propertyName, out var result))
            {
                return result;
            }

            var productType = product.GetType();
            var productProperty = productType.GetProperty(propertyName);
            result = productProperty?.GetSetMethod();

            _productProperties.Add(propertyName, result);

            return result;
        }

        protected virtual IEnumerable<Property> GetStandardProperties()
        {
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.Name),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.LongText,
                                 Required = true
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.StartDate),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.DateTime,
                                 Required = true,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.EndDate),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.DateTime,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.Priority),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Integer,
                                 Required = true,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.EnableReview),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.IsActive),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.IsBuyable),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.TrackInventory),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Boolean,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.MinQuantity),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Integer,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.MaxQuantity),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Integer,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.Vendor),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.WeightUnit),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.Weight),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.MeasureUnit),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.PackageType),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.ShortText,
                                 Dictionary = true
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.Height),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.Width),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.Length),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                             };
            yield return new Property
                             {
                                 Name = nameof(CatalogProduct.TaxType),
                                 Type = PropertyType.Product,
                                 ValueType = PropertyValueType.Number,
                                 Dictionary = true
                             };
        }

        protected virtual bool SetCustomProperty(CatalogProduct product, moduleCoreModels.Property propertyToSet)
        {
            bool result;

            if (propertyToSet.Multivalue)
            {
                var propertyValues = product.PropertyValues?.Where(
                    propertyValue => propertyValue.Property != null
                                     && propertyValue.Property.Id.EqualsInvariant(propertyToSet.Id)).ToArray();

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

                result = AddPropertyValues(product, propertyToSet);
            }
            else
            {
                var productPropertyValue = product.PropertyValues?.FirstOrDefault(
                    propertyValue => propertyValue.Property != null
                                     && propertyValue.Property.Id.EqualsInvariant(propertyToSet.Id));

                if (productPropertyValue != null)
                {
                    var propertyValue = propertyToSet.Values.FirstOrDefault();

                    productPropertyValue.Value = propertyValue?.Value;

                    if (propertyToSet.Dictionary)
                    {
                        productPropertyValue.ValueId = propertyValue?.ValueId;
                    }

                    result = true;
                }
                else
                {
                    result = AddPropertyValues(product, propertyToSet);
                }
            }

            return result;
        }

        protected virtual bool SetOwnProperty(CatalogProduct product, moduleCoreModels.Property property)
        {
            bool result;
            var propertyValue = property.Values.FirstOrDefault();
            var value = property.Dictionary ? propertyValue?.ValueId : propertyValue?.Value;
            var setter = GetProductPropertySetter(product, property);

            if (setter == null)
            {
                result = false;
            }
            else
            {
                if (value == null && property.Required)
                {
                    throw new ArgumentException(
                        $"Property value is missing for required property \"{property.Name}\".");
                }

                var convertedValue = value != null ? ConvertValue(property.ValueType, value) : null;

                setter.Invoke(product, new[] { convertedValue });
                result = true;
            }

            return result;
        }

        private static bool AddPropertyValues(IHasProperties product, moduleCoreModels.Property propertyToSet)
        {
            bool result;
            var property = product.Properties.FirstOrDefault(p => p.Id.EqualsInvariant(propertyToSet.Id));
            if (property == null)
            {
                result = false;
            }
            else
            {
                if (product.PropertyValues == null)
                {
                    product.PropertyValues = new List<PropertyValue>();
                }

                foreach (var propertyValue in propertyToSet.Values.Select(value => value.ToCoreModel()))
                {
                    propertyValue.Property = property;
                    propertyValue.PropertyId = property.Id;
                    propertyValue.PropertyName = property.Name;
                    product.PropertyValues.Add(propertyValue);
                }

                result = true;
            }

            return result;
        }
    }
}