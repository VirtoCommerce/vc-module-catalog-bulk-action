namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionAbstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Property = VirtoCommerce.CatalogBulkActionsModule.Core.Models.Property;

    public class PropertiesUpdateBulkAction : IBulkAction
    {
        private readonly IBulkPropertyUpdateManager _bulkPropertyUpdateManager;

        private readonly ICatalogService _catalogService;

        private readonly ICategoryService _categoryService;

        private readonly PropertiesUpdateBulkActionContext _context;

        private readonly IItemService _itemService;

        private readonly Dictionary<string, string> _namesById = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesUpdateBulkAction"/> class.
        /// </summary>
        /// <param name="bulkPropertyUpdateManager">
        /// The bulk update property manager.
        /// </param>
        /// <param name="itemService">
        /// The item service.
        /// </param>
        /// <param name="catalogService">
        /// The catalog service.
        /// </param>
        /// <param name="categoryService">
        /// The category service.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public PropertiesUpdateBulkAction(
            IBulkPropertyUpdateManager bulkPropertyUpdateManager,
            IItemService itemService,
            ICatalogService catalogService,
            ICategoryService categoryService,
            PropertiesUpdateBulkActionContext context)
        {
            _bulkPropertyUpdateManager = bulkPropertyUpdateManager;
            _itemService = itemService;
            _catalogService = catalogService;
            _categoryService = categoryService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BulkActionContext Context => _context;

        public BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var entries = entities.Cast<ListEntry>().ToArray();

            if (entries.Any(entry => !entry.Type.EqualsInvariant(ListEntryProduct.TypeName)))
            {
                throw new ArgumentException($"{GetType().Name} could be applied to product entities only.");
            }

            var productIds = entries.Where(entry => entry.Type.EqualsInvariant(ListEntryProduct.TypeName))
                .Select(entry => entry.Id).ToArray();
            var products = _itemService.GetByIds(
                productIds,
                ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties);

            return _bulkPropertyUpdateManager.UpdateProperties(products, _context.Properties);
        }

        public object GetActionData()
        {
            var properties = _bulkPropertyUpdateManager.GetProperties(_context);

            return new { Properties = properties.Select(CreateModel).ToArray() };
        }

        public BulkActionResult Validate()
        {
            return BulkActionResult.Success;
        }

        private Property CreateModel(Domain.Catalog.Model.Property property)
        {
            var result = property.ToWebModel();
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
                        var catalog = _catalogService.GetById(property.CatalogId);
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
                    var category = _categoryService.GetById(property.CategoryId, CategoryResponseGroup.Info);
                    path = $"{category?.Name} (Category)";
                    _namesById.Add(property.CategoryId, path);
                }
            }

            result.Path = path;

            return result;
        }
    }
}