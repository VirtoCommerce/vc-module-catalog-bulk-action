namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public interface ISearchService
    {
        SearchResult Search(VirtoCommerce.Domain.Catalog.Model.SearchCriteria criteria);
    }
}