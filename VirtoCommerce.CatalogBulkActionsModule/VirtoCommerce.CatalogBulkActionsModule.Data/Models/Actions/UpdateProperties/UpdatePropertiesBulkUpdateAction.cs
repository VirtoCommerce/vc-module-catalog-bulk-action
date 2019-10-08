namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Property = VirtoCommerce.CatalogBulkActionsModule.Core.Models.Property;

    public class UpdatePropertiesBulkUpdateAction : IBulkUpdateAction
    {
        private readonly IBulkUpdatePropertyManager _bulkUpdatePropertyManager;

        private readonly ICatalogService _catalogService;

        private readonly ICategoryService _categoryService;

        private readonly UpdatePropertiesActionContext _context;

        private readonly IItemService _itemService;

        private readonly Dictionary<string, string> _namesById = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePropertiesBulkUpdateAction"/> class.
        /// </summary>
        /// <param name="bulkUpdatePropertyManager">
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
        public UpdatePropertiesBulkUpdateAction(
            IBulkUpdatePropertyManager bulkUpdatePropertyManager,
            IItemService itemService,
            ICatalogService catalogService,
            ICategoryService categoryService,
            UpdatePropertiesActionContext context)
        {
            _bulkUpdatePropertyManager = bulkUpdatePropertyManager;
            _itemService = itemService;
            _catalogService = catalogService;
            _categoryService = categoryService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BulkUpdateActionContext Context => _context;

        public virtual BulkUpdateActionResult Execute(IEnumerable<IEntity> entities)
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

            return _bulkUpdatePropertyManager.UpdateProperties(products, _context.Properties);
        }

        public virtual IBulkUpdateActionData GetActionData()
        {
            var properties = _bulkUpdatePropertyManager.GetProperties(_context);

            return new UpdatePropertiesActionData { Properties = properties.Select(CreateWebModel).ToArray() };
        }

        public virtual BulkUpdateActionResult Validate()
        {
            var result = BulkUpdateActionResult.Success;
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