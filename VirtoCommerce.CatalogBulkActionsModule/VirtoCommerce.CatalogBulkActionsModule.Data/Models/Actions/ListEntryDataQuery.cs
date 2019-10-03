namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public class ListEntryDataQuery
    {
        public ListEntry[] ListEntries { get; set; }

        public SearchCriteria SearchCriteria { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}