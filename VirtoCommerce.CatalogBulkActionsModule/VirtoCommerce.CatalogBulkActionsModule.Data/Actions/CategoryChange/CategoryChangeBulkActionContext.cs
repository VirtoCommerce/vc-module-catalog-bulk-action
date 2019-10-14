﻿namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange
{
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;

    public class CategoryChangeBulkActionContext : BulkActionContext
    {
        /// <summary>
        /// Gets or sets the catalog id.
        /// </summary>
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        public string CategoryId { get; set; }
    }
}