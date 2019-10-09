namespace VirtoCommerce.CatalogBulkActionsModule.Core.Services
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public interface ISearchService
    {
        SearchResult Search(VirtoCommerce.Domain.Catalog.Model.SearchCriteria criteria);
    }
}