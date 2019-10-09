namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions
{
    public interface IBulkActionFactory
    {
        IBulkAction Create(BulkActionContext context);
    }
}