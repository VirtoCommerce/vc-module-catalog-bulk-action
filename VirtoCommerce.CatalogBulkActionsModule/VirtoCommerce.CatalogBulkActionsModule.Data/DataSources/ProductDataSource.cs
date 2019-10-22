namespace VirtoCommerce.CatalogBulkActionsModule.Data.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Platform.Core.Common;

    public class ProductDataSource : BaseDataSource
    {
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDataSource"/> class.
        /// </summary>
        /// <param name="searchService">
        /// The list entry search service.
        /// </param>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        public ProductDataSource(ISearchService searchService, DataQuery dataQuery)
            : base(searchService, dataQuery)
        {
            _searchService = searchService;
        }

        protected override SearchCriteria BuildSearchCriteria(DataQuery dataQuery)
        {
            var result = base.BuildSearchCriteria(dataQuery);
            result.ResponseGroup = SearchResponseGroup.WithProducts;
            result.SearchInChildren = true;
            return result;
        }

        protected override IEnumerable<IEntity> GetNextItems(IEnumerable<ListEntry> entries, int skip, int take)
        {
            var result = new List<IEntity>();
            var categoryProductsSkip = 0;
            var categoryProductsTake = 0;

            var categoryIds = entries.Where(entry => entry.Type.EqualsInvariant(ListEntryCategory.TypeName))
                .Select(entry => entry.Id).ToArray();
            if (categoryIds.IsNullOrEmpty())
            {
                // idle
            }
            else
            {
                // find all products inside category entries
                var searchResult = SearchProductsInCategories(categoryIds, 0, 0);
                var inCategoriesCount = searchResult.TotalCount;

                categoryProductsSkip = Math.Min(inCategoriesCount, skip);
                categoryProductsTake = Math.Min(take, Math.Max(0, inCategoriesCount - skip));

                if (inCategoriesCount > 0 && categoryProductsTake > 0)
                {
                    searchResult = SearchProductsInCategories(categoryIds, categoryProductsSkip, categoryProductsTake);
                    result.AddRange(searchResult.Entries);
                }
            }

            skip -= categoryProductsSkip;
            take -= categoryProductsTake;

            var products = entries.Where(entry => entry.Type.EqualsInvariant(ListEntryProduct.TypeName)).Skip(skip)
                .Take(take).ToArray();
            result.AddRange(products);

            return result;
        }

        protected override int GetEntitiesCount(IEnumerable<ListEntry> entries)
        {
            var inCategoriesCount = 0;
            var categoryIds = entries.Where(entry => entry.Type.EqualsInvariant(ListEntryCategory.TypeName))
                .Select(entry => entry.Id).ToArray();

            if (categoryIds.IsNullOrEmpty())
            {
                // idle
            }
            else
            {
                // find all products inside category entries
                var searchResult = SearchProductsInCategories(categoryIds, 0, 0);
                inCategoriesCount = searchResult.TotalCount;
            }

            // find product list entry count
            var productCount = entries.Count(entry => entry.Type.EqualsInvariant(ListEntryProduct.TypeName));

            return inCategoriesCount + productCount;
        }

        private Core.Models.SearchResult SearchProductsInCategories(string[] categoryIds, int skip, int take)
        {
            var searchCriteria = new SearchCriteria
                                     {
                                         CategoryIds = categoryIds,
                                         Skip = skip,
                                         Take = take,
                                         ResponseGroup = SearchResponseGroup.WithProducts,
                                         SearchInChildren = true,
                                         SearchInVariations = true
                                     };

            var result = _searchService.Search(searchCriteria);
            return result;
        }
    }
}