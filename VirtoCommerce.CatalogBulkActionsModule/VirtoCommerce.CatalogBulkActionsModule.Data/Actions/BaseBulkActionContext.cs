namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions
{
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public class BaseBulkActionContext : BulkActionContext
    {
        public DataQuery DataQuery { get; set; }
    }
}