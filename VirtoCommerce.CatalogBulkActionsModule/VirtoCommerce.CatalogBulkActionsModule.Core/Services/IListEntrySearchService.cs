namespace VirtoCommerce.CatalogBulkActionsModule.Core.Services
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    using SearchCriteria = VirtoCommerce.Domain.Catalog.Model.SearchCriteria;

    public interface IListEntrySearchService
    {
        ListEntrySearchResult Search(SearchCriteria criteria);
    }
}