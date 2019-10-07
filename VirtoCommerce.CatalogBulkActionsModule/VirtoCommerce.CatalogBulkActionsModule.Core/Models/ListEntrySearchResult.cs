namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Class representing the result of ListEntries search.
    /// </summary>
    public class ListEntrySearchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListEntrySearchResult"/> class.
        /// </summary>
        public ListEntrySearchResult()
        {
            ListEntries = new List<ListEntry>();
        }

        /// <summary>
        /// Gets or sets the list entries.
        /// </summary>
        /// <value>
        /// The list entries.
        /// </value>
        public ICollection<ListEntry> ListEntries { get; set; }

        /// <summary>
        /// Gets or sets the total entries count matching the search criteria.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }
    }
}