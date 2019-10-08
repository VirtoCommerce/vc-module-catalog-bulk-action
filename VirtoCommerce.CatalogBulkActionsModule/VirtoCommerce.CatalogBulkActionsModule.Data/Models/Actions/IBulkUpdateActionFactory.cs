namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    public interface IBulkUpdateActionFactory
    {
        IBulkUpdateAction Create(BulkUpdateActionContext context);
    }
}