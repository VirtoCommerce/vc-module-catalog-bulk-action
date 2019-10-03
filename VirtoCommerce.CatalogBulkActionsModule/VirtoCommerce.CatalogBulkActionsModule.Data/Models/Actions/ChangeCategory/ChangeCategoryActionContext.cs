namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.ChangeCategory
{
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public class ChangeCategoryActionContext : BulkUpdateActionContext
    {
        public string CategoryId { get; set; }
        public string CatalogId { get; set; }
        public ListEntryDataQuery DataQuery { get; set; }
    }
}