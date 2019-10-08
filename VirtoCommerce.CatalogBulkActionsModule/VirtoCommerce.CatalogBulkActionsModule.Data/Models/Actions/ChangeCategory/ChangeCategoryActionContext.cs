namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.ChangeCategory
{
    public class ChangeCategoryActionContext : BulkUpdateActionContext
    {
        /// <summary>
        /// Gets or sets the catalog id.
        /// </summary>
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the data query.
        /// </summary>
        public ListEntryDataQuery DataQuery { get; set; }
    }
}