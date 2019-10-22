namespace VirtoCommerce.CatalogBulkActionsModule.Data.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Platform.Core.Common;

    public class BaseDataSource : IDataSource
    {
        private readonly DataQuery _dataQuery;

        private readonly int _pageSize = 50;

        private readonly ISearchService _searchService;

        private int _pageNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataSource"/> class.
        /// </summary>
        /// <param name="searchService">
        /// The search service.
        /// </param>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        public BaseDataSource(ISearchService searchService, DataQuery dataQuery)
        {
            _searchService = searchService;
            _dataQuery = dataQuery ?? throw new ArgumentNullException(nameof(dataQuery));
        }

        public IEnumerable<IEntity> Items { get; protected set; }

        public bool Fetch()
        {
            if (_dataQuery.ListEntries.IsNullOrEmpty())
            {
                if (_dataQuery.SearchCriteria == null)
                {
                    Items = Array.Empty<IEntity>();
                }
                else
                {
                    var searchCriteria = BuildSearchCriteria(_dataQuery);
                    var searchResult = _searchService.Search(searchCriteria);
                    Items = searchResult.Entries;
                }
            }
            else
            {
                var skip = GetSkip();
                var take = GetTake();
                var entities = GetNextItems(_dataQuery.ListEntries, skip, take);
                Items = entities.ToArray();
            }

            _pageNumber++;

            return Items.Any();
        }

        public virtual int GetTotalCount()
        {
            var result = 0;

            if (_dataQuery.ListEntries.IsNullOrEmpty())
            {
                if (_dataQuery.SearchCriteria == null)
                {
                    // idle
                }
                else
                {
                    var searchCriteria = BuildSearchCriteria(_dataQuery);
                    searchCriteria.Skip = 0;
                    searchCriteria.Take = 0;
                    var searchResult = _searchService.Search(searchCriteria);
                    result = searchResult.TotalCount;
                }
            }
            else
            {
                result = GetEntitiesCount(_dataQuery.ListEntries);
            }

            return result;
        }

        protected virtual SearchCriteria BuildSearchCriteria(DataQuery dataQuery)
        {
            if (dataQuery.SearchCriteria == null)
            {
                dataQuery.SearchCriteria = new SearchCriteria();
            }

            var result = dataQuery.SearchCriteria;

            result.Skip = GetSkip();
            result.Take = GetTake();
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

        protected virtual IEnumerable<IEntity> GetNextItems(IEnumerable<ListEntry> entries, int skip, int take)
        {
            return entries.Skip(skip).Take(take);
        }

        protected virtual int GetEntitiesCount(IEnumerable<ListEntry> entries)
        {
            return entries.Count();
        }

        private int GetSkip()
        {
            var skip = _dataQuery.Skip.GetValueOrDefault();
            return (_pageNumber * _pageSize) + skip;
        }

        private int GetTake()
        {
            return _dataQuery.Take.GetValueOrDefault(_pageSize);
        }
    }
}