namespace VirtoCommerce.CatalogBulkActionsModule.Core.Services
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public interface IListEntrySearchService
    {
        ListEntrySearchResult Search(Domain.Catalog.Model.SearchCriteria criteria);
    }
}