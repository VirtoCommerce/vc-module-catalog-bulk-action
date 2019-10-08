namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public class ListEntryDataQuery
    {
        /// <summary>
        /// Gets or sets the list entries.
        /// </summary>
        public ListEntry[] ListEntries { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        public SearchCriteria SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the skip.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        public int? Take { get; set; }
    }
}