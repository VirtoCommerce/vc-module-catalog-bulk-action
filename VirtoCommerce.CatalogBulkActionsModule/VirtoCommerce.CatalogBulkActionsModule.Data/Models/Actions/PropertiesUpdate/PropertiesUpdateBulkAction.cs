namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.PropertiesUpdate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Property = VirtoCommerce.CatalogBulkActionsModule.Core.Models.Property;

    public class PropertiesUpdateBulkAction : IBulkAction
    {
        private readonly IBulkPropertyUpdateManager bulkPropertyUpdateManager;

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
            this.bulkPropertyUpdateManager = bulkPropertyUpdateManager;
            _itemService = itemService;
            _catalogService = catalogService;
            _categoryService = categoryService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BulkActionContext Context => _context;

        public virtual BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var listEntries = entities.Cast<ListEntry>().ToArray();

            if (listEntries.Any(entry => !entry.Type.EqualsInvariant(ListEntryProduct.TypeName)))
            {
                throw new ArgumentException($"{GetType().Name} could be applied to product entities only.");
            }

            var productIds = listEntries.Where(entry => entry.Type.EqualsInvariant(ListEntryProduct.TypeName))
                .Select(entry => entry.Id).ToArray();
            var products = _itemService.GetByIds(
                productIds,
                ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties);

            return bulkPropertyUpdateManager.UpdateProperties(products, _context.Properties);
        }

        public virtual object GetActionData()
        {
            var properties = bulkPropertyUpdateManager.GetProperties(_context);

            return new { Properties = properties.Select(CreateWebModel).ToArray() };
        }

        public virtual BulkActionResult Validate()
        {
            var result = BulkActionResult.Success;
            return result;
        }

        protected virtual Property CreateWebModel(Domain.Catalog.Model.Property property)
        {
            var result = property.ToWebModel();
            string ownerName;

            if (string.IsNullOrEmpty(property.CategoryId))
            {
                if (string.IsNullOrEmpty(property.CatalogId))
                {
                    ownerName = "Native properties";
                }
                else
                {
                    if (_namesById.TryGetValue(property.CatalogId, out ownerName))
                    {
                        // idle
                    }
                    else
                    {
                        ownerName = $"{_catalogService.GetById(property.CatalogId)?.Name} (Catalog)";
                        _namesById.Add(property.CatalogId, ownerName);
                    }
                }
            }
            else
            {
                if (_namesById.TryGetValue(property.CategoryId, out ownerName))
                {
                    // idle
                }
                else
                {
                    ownerName =
                        $"{_categoryService.GetById(property.CategoryId, CategoryResponseGroup.Info)?.Name} (Category)";
                    _namesById.Add(property.CategoryId, ownerName);
                }
            }

            result.Path = ownerName;

            return result;
        }
    }
}