namespace VirtoCommerce.CatalogBulkActionsModule.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Assets;
    using VirtoCommerce.Platform.Core.Common;

    using Flags = VirtoCommerce.Domain.Catalog.Model.SearchResponseGroup;
    using SearchResult = VirtoCommerce.CatalogBulkActionsModule.Core.Models.SearchResult;

    public class SearchService : ISearchService
    {
        private readonly IServiceProvider _lazyServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchService"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        public SearchService(IServiceProvider serviceProvider)
        {
            _lazyServiceProvider = serviceProvider;
        }

        public SearchResult Search(SearchCriteria criteria)
        {
            var result = new SearchResult();
            var searchService = _lazyServiceProvider.Resolve<ICatalogSearchService>();
            var blobUrlResolver = _lazyServiceProvider.Resolve<IBlobUrlResolver>();
            var skip = 0;
            var take = 0;

            // because products and categories represent in search result as two separated collections for handle paging request 
            // we should join two resulting collection artificially

            // search categories
            var responseGroupCopy = criteria.ResponseGroup;
            if (criteria.ResponseGroup.HasFlag(Flags.WithCategories))
            {
                criteria.ResponseGroup &= ~Flags.WithProducts;

                var searchResult = searchService.Search(criteria);
                var totalCount = searchResult.Categories.Count;
                skip = GetSkip(criteria, totalCount);
                take = GetTake(criteria, totalCount);
                var pagedCategories = GetPaged(searchResult.Categories, skip, take);
                var categories = pagedCategories.Select(
                    category => new ListEntryCategory(category.ToWebModel(blobUrlResolver)));
                result.TotalCount = totalCount;

                result.Entries.AddRange(categories);
            }

            criteria.ResponseGroup = responseGroupCopy;

            // search products
            if (criteria.ResponseGroup.HasFlag(Flags.WithProducts))
            {
                criteria.ResponseGroup &= ~Flags.WithCategories;

                criteria.Skip -= skip;
                criteria.Take -= take;
                var searchResult = searchService.Search(criteria);
                var products = searchResult.Products.Select(
                    product => new ListEntryProduct(product.ToWebModel(blobUrlResolver)));
                result.TotalCount += searchResult.ProductsTotalCount;

                result.Entries.AddRange(products);
            }

            return result;
        }

        private static IEnumerable<T> GetPaged<T>(IEnumerable<T> collection, int skip, int take)
        {
            return collection.Skip(skip).Take(take);
        }

        private static int GetSkip(SearchCriteria criteria, int totalCount)
        {
            return Math.Min(totalCount, criteria.Skip);
        }

        private static int GetTake(SearchCriteria criteria, int totalCount)
        {
            return Math.Min(criteria.Take, Math.Max(0, totalCount - criteria.Skip));
        }
    }
}