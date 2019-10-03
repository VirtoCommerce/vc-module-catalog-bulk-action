namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.ChangeCategory;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties;

    public class BulkUpdateDataSourceFactory : IPagedDataSourceFactory
    {
        private readonly IListEntrySearchService _searchService;

        public BulkUpdateDataSourceFactory(IListEntrySearchService searchService)
        {
            _searchService = searchService;
        }

        public IPagedDataSource Create(BulkUpdateActionContext context)
        {
            IPagedDataSource result = null;

            if (context is ChangeCategoryActionContext changeCategoryActionContext)
            {
                result = new ListEntryPagedDataSource(_searchService, changeCategoryActionContext.DataQuery);
            }
            else if (context is UpdatePropertiesActionContext bulkUpdateActionContext)
            {
                result = new ListEntryProductPagedDataSource(_searchService, bulkUpdateActionContext.DataQuery);
            }

            return result ?? throw new ArgumentException($"Unsupported bulk update query type: {context.GetType().Name}");
        }
    }
}