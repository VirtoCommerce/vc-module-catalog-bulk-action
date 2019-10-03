namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    public abstract class BulkUpdateActionContext
    {
        public string ActionName { get; set; }
        public string ContextTypeName => GetType().Name;
    }
}