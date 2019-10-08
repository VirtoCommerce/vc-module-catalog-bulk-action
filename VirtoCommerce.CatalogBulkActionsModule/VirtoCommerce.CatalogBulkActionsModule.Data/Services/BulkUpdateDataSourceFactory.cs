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

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdateDataSourceFactory"/> class.
        /// </summary>
        /// <param name="searchService">
        /// The search service.
        /// </param>
        public BulkUpdateDataSourceFactory(IListEntrySearchService searchService)
        {
            _searchService = searchService;
        }

        public IPagedDataSource Create(BulkUpdateActionContext context)
        {
            IPagedDataSource result = null;

            switch (context)
            {
                case ChangeCategoryActionContext changeCategoryActionContext:
                    result = new ListEntryPagedDataSource(_searchService, changeCategoryActionContext.DataQuery);
                    break;

                case UpdatePropertiesActionContext bulkUpdateActionContext:
                    result = new ListEntryProductPagedDataSource(_searchService, bulkUpdateActionContext.DataQuery);
                    break;
            }

            return result ?? throw new ArgumentException(
                       $"Unsupported bulk update query type: {context.GetType().Name}");
        }
    }
}