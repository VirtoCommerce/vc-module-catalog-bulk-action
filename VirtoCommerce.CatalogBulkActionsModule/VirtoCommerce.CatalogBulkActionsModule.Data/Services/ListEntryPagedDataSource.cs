﻿namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Converters;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.Platform.Core.Common;

    using domain = VirtoCommerce.Domain.Catalog.Model;

    public class ListEntryPagedDataSource : IPagedDataSource
    {
        private readonly IListEntrySearchService _searchService;

        public ListEntryPagedDataSource(IListEntrySearchService searchService, ListEntryDataQuery dataQuery)
        {
            _searchService = searchService;
            DataQuery = dataQuery ?? throw new ArgumentNullException(nameof(dataQuery));
        }

        public int PageSize { get; set; } = 50;
        public IEnumerable<IEntity> Items { get; protected set; }
        public int CurrentPageNumber { get; protected set; }

        public ListEntryDataQuery DataQuery { get; protected set; }

        public virtual bool Fetch()
        {
            if (!DataQuery.ListEntries.IsNullOrEmpty())
            {
                var (skip, take) = GetSkipTake();
                var entities = GetEntities(DataQuery.ListEntries, skip, take);
                Items = entities.ToArray();
            }
            else if (DataQuery.SearchCriteria != null)
            {
                var domainSearchCriteria = BuildSearchCriteria(DataQuery);
                var searchResult = _searchService.Search(domainSearchCriteria);
                Items = searchResult.ListEntries;
            }
            else
            {
                Items = Array.Empty<IEntity>();
            }

            CurrentPageNumber++;

            return Items.Any();
        }

        public virtual int GetTotalCount()
        {
            var result = 0;

            if (!DataQuery.ListEntries.IsNullOrEmpty())
            {
                result = GetEntitiesCount(DataQuery.ListEntries);
            }
            else if (DataQuery.SearchCriteria != null)
            {
                var domainSearchCriteria = BuildSearchCriteria(DataQuery);

                domainSearchCriteria.Skip = 0;
                domainSearchCriteria.Take = 0;

                var searchResult = _searchService.Search(domainSearchCriteria);

                result = searchResult.TotalCount;
            }

            return result;
        }

        protected virtual IEnumerable<IEntity> GetEntities(IEnumerable<ListEntry> listEntries, int skip, int take)
        {
            return listEntries.Skip(skip).Take(take);
        }

        protected virtual int GetEntitiesCount(IEnumerable<ListEntry> listEntries)
        {
            return listEntries.Count();
        }

        protected (int, int) GetSkipTake()
        {
            var skip = (DataQuery.Skip ?? 0) + CurrentPageNumber * PageSize;
            var take = DataQuery.Take ?? PageSize;

            return (skip, take);
        }

        protected virtual domain.SearchCriteria BuildSearchCriteria(ListEntryDataQuery dataQuery)
        {
            var result = dataQuery.SearchCriteria.ToCoreModel();
            var (skip, take) = GetSkipTake();

            result.Skip = skip;
            result.Take = take;

            result.WithHidden = true;
            //Need search in children categories if user specify keyword
            if (!string.IsNullOrEmpty(result.Keyword))
            {
                result.SearchInChildren = true;
                result.SearchInVariations = true;
            }

            return result;
        }
    }
}