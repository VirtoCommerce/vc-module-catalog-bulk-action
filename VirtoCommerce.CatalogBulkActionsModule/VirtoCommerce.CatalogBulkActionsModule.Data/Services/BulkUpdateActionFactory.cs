namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.ChangeCategory;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;

    public class BulkUpdateActionFactory : IBulkUpdateActionFactory
    {
        private readonly IBulkUpdatePropertyManager _bulkUpdatePropertyManager;

        private readonly ICatalogService _catalogService;

        private readonly IListEntryMover<Category> _categoryMover;

        private readonly ICategoryService _categoryService;

        private readonly IItemService _itemService;

        private readonly IListEntryMover<CatalogProduct> _productMover;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdateActionFactory"/> class.
        /// </summary>
        /// <param name="catalogService">
        /// The catalog service.
        /// </param>
        /// <param name="categoryMover">
        /// The category mover.
        /// </param>
        /// <param name="productMover">
        /// The product mover.
        /// </param>
        /// <param name="bulkUpdatePropertyManager">
        /// The bulk update property manager.
        /// </param>
        /// <param name="itemService">
        /// The item service.
        /// </param>
        /// <param name="categoryService">
        /// The category service.
        /// </param>
        public BulkUpdateActionFactory(
            ICatalogService catalogService,
            IListEntryMover<Category> categoryMover,
            IListEntryMover<CatalogProduct> productMover,
            IBulkUpdatePropertyManager bulkUpdatePropertyManager,
            IItemService itemService,
            ICategoryService categoryService)
        {
            _catalogService = catalogService;
            _categoryMover = categoryMover;
            _productMover = productMover;
            _bulkUpdatePropertyManager = bulkUpdatePropertyManager;
            _itemService = itemService;
            _categoryService = categoryService;
        }

        public IBulkUpdateAction Create(BulkUpdateActionContext context)
        {
            IBulkUpdateAction result = null;

            switch (context)
            {
                case ChangeCategoryActionContext changeCategoryActionContext:
                    result = new ChangeCategoryBulkUpdateAction(
                        _catalogService,
                        _categoryMover,
                        _productMover,
                        changeCategoryActionContext);
                    break;

                case UpdatePropertiesActionContext updatePropertiesActionContext:
                    result = new UpdatePropertiesBulkUpdateAction(
                        _bulkUpdatePropertyManager,
                        _itemService,
                        _catalogService,
                        _categoryService,
                        updatePropertiesActionContext);
                    break;
            }

            return result ?? throw new ArgumentException($"Unsupported action type: {context.GetType().Name}");
        }
    }
}