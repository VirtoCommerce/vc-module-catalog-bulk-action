namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;

    using IServiceProvider = VirtoCommerce.CatalogBulkActionsModule.Core.IServiceProvider;

    public class BulkActionFactory : IBulkActionFactory
    {
        private readonly IBulkPropertyUpdateManager _bulkPropertyUpdateManager;

        private readonly IMover<Category> _categoryMover;

        private readonly IMover<CatalogProduct> _productMover;
        
        private readonly IServiceProvider _lazyServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkActionFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <param name="categoryMover">
        /// The category mover.
        /// </param>
        /// <param name="productMover">
        /// The product mover.
        /// </param>
        /// <param name="bulkPropertyUpdateManager">
        /// The bulk property update manager.
        /// </param>
        public BulkActionFactory(
            IServiceProvider serviceProvider,
            IMover<Category> categoryMover,
            IMover<CatalogProduct> productMover,
            IBulkPropertyUpdateManager bulkPropertyUpdateManager)
        {
            _lazyServiceProvider = serviceProvider;
            _categoryMover = categoryMover;
            _productMover = productMover;
            _bulkPropertyUpdateManager = bulkPropertyUpdateManager;
        }

        public IBulkAction Create(BulkActionContext context)
        {
            IBulkAction result = null;

            var catalogService = _lazyServiceProvider.Resolve<ICatalogService>();
            var itemService = _lazyServiceProvider.Resolve<IItemService>();
            var categoryService = _lazyServiceProvider.Resolve<ICategoryService>();

            switch (context)
            {
                case CategoryChangeBulkActionContext changeCategoryActionContext:
                    result = new CategoryChangeBulkAction(
                        catalogService,
                        _categoryMover,
                        _productMover,
                        changeCategoryActionContext);
                    break;

                case PropertiesUpdateBulkActionContext updatePropertiesActionContext:
                    result = new PropertiesUpdateBulkAction(
                        _bulkPropertyUpdateManager,
                        itemService,
                        catalogService,
                        categoryService,
                        updatePropertiesActionContext);
                    break;
            }

            return result ?? throw new ArgumentException($"Unsupported action type: {context.GetType().Name}");
        }
    }
}