namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.CatalogModule.Web.Converters;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Property = VirtoCommerce.CatalogModule.Web.Model;

    public class PropertiesUpdateBulkAction : IBulkAction
    {
        private readonly PropertiesUpdateBulkActionContext _context;

        private readonly ILazyServiceProvider _lazyServiceProvider;

        private readonly Dictionary<string, string> _namesById = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesUpdateBulkAction"/> class.
        /// </summary>
        /// <param name="lazyServiceProvider">
        /// The lazy service provider.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public PropertiesUpdateBulkAction(ILazyServiceProvider lazyServiceProvider, PropertiesUpdateBulkActionContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _lazyServiceProvider = lazyServiceProvider;
        }

        public BulkActionContext Context => _context;

        public BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var itemService = _lazyServiceProvider.Resolve<IItemService>();
            var bulkPropertyUpdateManager = _lazyServiceProvider.Resolve<IBulkPropertyUpdateManager>();
            var entries = entities.Cast<ListEntry>().ToArray();

            if (entries.Any(entry => !entry.Type.EqualsInvariant(ListEntryProduct.TypeName)))
            {
                throw new ArgumentException($"{GetType().Name} could be applied to product entities only.");
            }

            var productIds = entries.Where(entry => entry.Type.EqualsInvariant(ListEntryProduct.TypeName))
                .Select(entry => entry.Id).ToArray();
            var products = itemService.GetByIds(
                productIds,
                ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties);

            return bulkPropertyUpdateManager.UpdateProperties(products, _context.Properties);
        }

        public object GetActionData()
        {
            var bulkPropertyUpdateManager = _lazyServiceProvider.Resolve<IBulkPropertyUpdateManager>();

            var properties = bulkPropertyUpdateManager.GetProperties(_context);

            return new { Properties = properties.Select(CreateModel).ToArray() };
        }

        public BulkActionResult Validate()
        {
            return BulkActionResult.Success;
        }

        private Property.Property CreateModel(Domain.Catalog.Model.Property property)
        {
            // TechDebt: in the called code we try to get access to validation rules and if they are have a null reference we will get an error
            property.ValidationRules = new List<PropertyValidationRule>();

            var result = property.ToWebModel();
            var catalogService = _lazyServiceProvider.Resolve<ICatalogService>();
            var categoryService = _lazyServiceProvider.Resolve<ICategoryService>();

            string path;

            if (string.IsNullOrEmpty(property.CategoryId))
            {
                if (string.IsNullOrEmpty(property.CatalogId))
                {
                    path = "Native properties";
                }
                else
                {
                    if (_namesById.TryGetValue(property.CatalogId, out path))
                    {
                        // idle
                    }
                    else
                    {
                        var catalog = catalogService.GetById(property.CatalogId);
                        path = $"{catalog?.Name} (Catalog)";
                        _namesById.Add(property.CatalogId, path);
                    }
                }
            }
            else
            {
                if (_namesById.TryGetValue(property.CategoryId, out path))
                {
                    // idle
                }
                else
                {
                    var category = categoryService.GetById(property.CategoryId, CategoryResponseGroup.Info);
                    path = $"{category?.Name} (Category)";
                    _namesById.Add(property.CategoryId, path);
                }
            }

            // TechDebt: verify this. For what we should use this path?
            //result.Path = path;

            return result;
        }
    }
}