namespace VirtoCommerce.CatalogBulkActionsModule.Core
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public interface ISearchService
    {
        SearchResult Search(VirtoCommerce.Domain.Catalog.Model.SearchCriteria criteria);
    }
}