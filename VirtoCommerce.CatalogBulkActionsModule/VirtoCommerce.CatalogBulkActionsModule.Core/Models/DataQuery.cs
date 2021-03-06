﻿namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using VirtoCommerce.CatalogModule.Web.Model;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public class DataQuery
    {
        /// <summary>
        /// Gets or sets the list entries.
        /// </summary>
        public ListEntry[] ListEntries { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        public VC.SearchCriteria SearchCriteria { get; set; }

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