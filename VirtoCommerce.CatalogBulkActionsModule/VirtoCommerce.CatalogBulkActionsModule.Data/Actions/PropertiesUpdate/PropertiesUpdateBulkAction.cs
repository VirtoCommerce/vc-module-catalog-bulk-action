namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Property = VirtoCommerce.CatalogBulkActionsModule.Core.Models.Property;

    public class PropertiesUpdateBulkAction : IBulkAction
    {
        private readonly PropertiesUpdateBulkActionContext _context;

        private readonly ILazyServiceProvider _lazyLazyServiceProvider;

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
            _lazyLazyServiceProvider = lazyServiceProvider;
        }

        public BulkActionContext Context => _context;

        public BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var itemService = _lazyLazyServiceProvider.Resolve<IItemService>();
            var bulkPropertyUpdateManager = _lazyLazyServiceProvider.Resolve<IBulkPropertyUpdateManager>();
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
            var bulkPropertyUpdateManager = _lazyLazyServiceProvider.Resolve<IBulkPropertyUpdateManager>();

            var properties = bulkPropertyUpdateManager.GetProperties(_context);

            return new { Properties = properties.Select(CreateModel).ToArray() };
        }

        public BulkActionResult Validate()
        {
            return BulkActionResult.Success;
        }

        private Property CreateModel(Domain.Catalog.Model.Property property)
        {
            var result = property.ToWebModel();
            var catalogService = _lazyLazyServiceProvider.Resolve<ICatalogService>();
            var categoryService = _lazyLazyServiceProvider.Resolve<ICategoryService>();

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

            result.Path = path;

            return result;
        }
    }
}