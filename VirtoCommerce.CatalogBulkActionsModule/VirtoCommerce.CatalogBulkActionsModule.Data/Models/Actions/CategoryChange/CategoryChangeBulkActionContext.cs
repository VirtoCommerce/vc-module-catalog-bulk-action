namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.CategoryChange
{
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