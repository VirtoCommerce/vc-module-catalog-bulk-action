namespace VirtoCommerce.CatalogBulkActionsModule.Data.DataSources
{
    using System;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogModule.Web.Services;

    public class DataSourceFactory : IDataSourceFactory
    {
        private readonly IListEntrySearchService _listEntrySearchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceFactory"/> class.
        /// </summary>
        /// <param name="listEntrySearchService">
        /// The search service.
        /// </param>
        public DataSourceFactory(IListEntrySearchService listEntrySearchService)
        {
            _listEntrySearchService = listEntrySearchService;
        }

        public IDataSource Create(BulkActionContext context)
        {
            IDataSource result = null;

            switch (context)
            {
                case CategoryChangeBulkActionContext categoryChangeContext:
                    result = new BaseDataSource(_listEntrySearchService, categoryChangeContext.DataQuery);
                    break;

                case PropertiesUpdateBulkActionContext propertiesUpdateContext:
                    result = new ProductDataSource(_listEntrySearchService, propertiesUpdateContext.DataQuery);
                    break;
            }

            var message = $"Unsupported bulk action query type: {context.GetType().Name}";
            return result ?? throw new ArgumentException(message);
        }
    }
}