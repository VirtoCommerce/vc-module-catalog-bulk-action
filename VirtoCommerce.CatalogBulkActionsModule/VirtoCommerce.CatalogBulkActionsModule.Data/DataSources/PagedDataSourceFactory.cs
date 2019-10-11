namespace VirtoCommerce.CatalogBulkActionsModule.Data.DataSources
{
    using System;

    using VirtoCommerce.BulkActionsModule.Core.BulkActionModels;
    using VirtoCommerce.BulkActionsModule.Core.DataSourceAbstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;

    public class PagedDataSourceFactory : IPagedDataSourceFactory
    {
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedDataSourceFactory"/> class.
        /// </summary>
        /// <param name="searchService">
        /// The search service.
        /// </param>
        public PagedDataSourceFactory(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public IPagedDataSource Create(BulkActionContext context)
        {
            IPagedDataSource result = null;

            switch (context)
            {
                case CategoryChangeBulkActionContext _:
                    result = new BasePagedDataSource(_searchService, context.DataQuery);
                    break;

                case PropertiesUpdateBulkActionContext _:
                    result = new ProductPagedDataSource(_searchService, context.DataQuery);
                    break;
            }

            var message = $"Unsupported bulk action query type: {context.GetType().Name}";
            return result ?? throw new ArgumentException(message);
        }
    }
}