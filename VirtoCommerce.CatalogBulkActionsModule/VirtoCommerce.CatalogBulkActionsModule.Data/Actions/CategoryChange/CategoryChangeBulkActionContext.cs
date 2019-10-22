﻿namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange
{
    public class CategoryChangeBulkActionContext : BaseBulkActionContext
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