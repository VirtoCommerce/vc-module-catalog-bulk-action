namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.ChangeCategory;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;

    public class BulkActionFactory : IBulkActionFactory
    {
        private readonly IBulkUpdateActionPropertyManager bulkUpdateActionPropertyManager;

        private readonly ICatalogService _catalogService;

        private readonly IMover<Category> _categoryMover;

        private readonly ICategoryService _categoryService;

        private readonly IItemService _itemService;

        private readonly IMover<CatalogProduct> _productMover;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkActionFactory"/> class.
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
        /// <param name="bulkUpdateActionPropertyManager">
        /// The bulk update property manager.
        /// </param>
        /// <param name="itemService">
        /// The item service.
        /// </param>
        /// <param name="categoryService">
        /// The category service.
        /// </param>
        public BulkActionFactory(
            ICatalogService catalogService,
            IMover<Category> categoryMover,
            IMover<CatalogProduct> productMover,
            IBulkUpdateActionPropertyManager bulkUpdateActionPropertyManager,
            IItemService itemService,
            ICategoryService categoryService)
        {
            _catalogService = catalogService;
            _categoryMover = categoryMover;
            _productMover = productMover;
            this.bulkUpdateActionPropertyManager = bulkUpdateActionPropertyManager;
            _itemService = itemService;
            _categoryService = categoryService;
        }

        public IBulkAction Create(BulkActionContext context)
        {
            IBulkAction result = null;

            switch (context)
            {
                case ChangeCategoryBulkActionContext changeCategoryActionContext:
                    result = new ChangeCategoryBulkAction(
                        _catalogService,
                        _categoryMover,
                        _productMover,
                        changeCategoryActionContext);
                    break;

                case UpdatePropertiesBulkActionContext updatePropertiesActionContext:
                    result = new UpdatePropertiesBulkAction(
                        bulkUpdateActionPropertyManager,
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