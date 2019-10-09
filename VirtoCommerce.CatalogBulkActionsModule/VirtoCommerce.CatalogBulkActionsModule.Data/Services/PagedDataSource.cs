namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;
    using VirtoCommerce.Platform.Core.Common;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public class PagedDataSource : IPagedDataSource
    {
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedDataSource"/> class.
        /// </summary>
        /// <param name="searchService">
        /// The search service.
        /// </param>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        public PagedDataSource(ISearchService searchService, ListEntryDataQuery dataQuery)
        {
            _searchService = searchService;
            DataQuery = dataQuery ?? throw new ArgumentNullException(nameof(dataQuery));
        }

        public int CurrentPageNumber { get; protected set; }

        public ListEntryDataQuery DataQuery { get; protected set; }

        public IEnumerable<IEntity> Items { get; protected set; }

        public int PageSize { get; set; } = 50;

        public virtual bool Fetch()
        {
            if (DataQuery.ListEntries.IsNullOrEmpty())
            {
                if (DataQuery.SearchCriteria == null)
                {
                    Items = Array.Empty<IEntity>();
                }
                else
                {
                    var searchCriteria = BuildSearchCriteria(DataQuery);
                    var searchResult = _searchService.Search(searchCriteria);
                    Items = searchResult.Entries;
                }
            }
            else
            {
                var (skip, take) = GetSkipTake();
                var entities = GetEntities(DataQuery.ListEntries, skip, take);
                Items = entities.ToArray();
            }

            CurrentPageNumber++;

            return Items.Any();
        }

        public virtual int GetTotalCount()
        {
            var result = 0;

            if (DataQuery.ListEntries.IsNullOrEmpty())
            {
                if (DataQuery.SearchCriteria == null)
                {
                    // idle
                }
                else
                {
                    var searchCriteria = BuildSearchCriteria(DataQuery);
                    searchCriteria.Skip = 0;
                    searchCriteria.Take = 0;
                    var searchResult = _searchService.Search(searchCriteria);
                    result = searchResult.TotalCount;
                }
            }
            else
            {
                result = GetEntitiesCount(DataQuery.ListEntries);
            }

            return result;
        }

        protected virtual VC.SearchCriteria BuildSearchCriteria(ListEntryDataQuery dataQuery)
        {
            var result = dataQuery.SearchCriteria.ToCoreModel();
            var (skip, take) = GetSkipTake();

            result.Skip = skip;
            result.Take = take;

            result.WithHidden = true;

            if (string.IsNullOrEmpty(result.Keyword))
            {
                return result;
            }

            // need search in children categories if user specify keyword
            result.SearchInChildren = true;
            result.SearchInVariations = true;
            return result;
        }

        protected virtual IEnumerable<IEntity> GetEntities(IEnumerable<ListEntry> entries, int skip, int take)
        {
            return entries.Skip(skip).Take(take);
        }

        protected virtual int GetEntitiesCount(IEnumerable<ListEntry> entries)
        {
            return entries.Count();
        }

        protected (int, int) GetSkipTake()
        {
            var skip = (DataQuery.Skip ?? 0) + CurrentPageNumber * PageSize;
            var take = DataQuery.Take ?? PageSize;
            return (skip, take);
        }
    }
}