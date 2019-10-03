namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models.Actions;

    public interface IBulkUpdateActionFactory
    {
        IBulkUpdateAction Create(BulkUpdateActionContext context);
    }
}