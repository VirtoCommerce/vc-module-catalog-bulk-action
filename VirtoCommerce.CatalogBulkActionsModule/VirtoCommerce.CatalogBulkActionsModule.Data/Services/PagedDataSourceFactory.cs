﻿namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.PropertiesUpdate;

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
                    result = new PagedDataSource(_searchService, context.DataQuery);
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