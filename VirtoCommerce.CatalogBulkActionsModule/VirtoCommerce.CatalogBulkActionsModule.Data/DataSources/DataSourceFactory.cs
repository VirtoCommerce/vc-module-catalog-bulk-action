namespace VirtoCommerce.CatalogBulkActionsModule.Data.DataSources
{
    using System;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;

    public class DataSourceFactory : IDataSourceFactory
    {
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceFactory"/> class.
        /// </summary>
        /// <param name="searchService">
        /// The search service.
        /// </param>
        public DataSourceFactory(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public IDataSource Create(BulkActionContext context)
        {
            IDataSource result = null;

            switch (context)
            {
                case CategoryChangeBulkActionContext categoryChangeContext:
                    result = new BaseDataSource(_searchService, categoryChangeContext.DataQuery);
                    break;

                case PropertiesUpdateBulkActionContext propertiesUpdateContext:
                    result = new ProductDataSource(_searchService, propertiesUpdateContext.DataQuery);
                    break;
            }

            var message = $"Unsupported bulk action query type: {context.GetType().Name}";
            return result ?? throw new ArgumentException(message);
        }
    }
}