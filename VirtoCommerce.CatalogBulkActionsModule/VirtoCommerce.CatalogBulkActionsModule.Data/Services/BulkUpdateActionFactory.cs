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
        private readonly ICatalogService _catalogService;
        private readonly IListEntryMover<Category> _categoryMover;
        private readonly IListEntryMover<CatalogProduct> _productMover;
        private readonly IBulkUpdatePropertyManager _bulkUpdatePropertyManager;
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;

        public BulkUpdateActionFactory(ICatalogService catalogService,
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

            if (context is ChangeCategoryActionContext changeCategoryActionContext)
            {
                result = new ChangeCategoryBulkUpdateAction(_catalogService, _categoryMover, _productMover, changeCategoryActionContext);
            }
            else if (context is UpdatePropertiesActionContext updatePropertiesActionContext)
            {
                result = new UpdatePropertiesBulkUpdateAction(_bulkUpdatePropertyManager, _itemService, _catalogService, _categoryService, updatePropertiesActionContext);
            }

            return result ?? throw new ArgumentException($"Unsupported action type: {context.GetType().Name}");
        }
    }
}