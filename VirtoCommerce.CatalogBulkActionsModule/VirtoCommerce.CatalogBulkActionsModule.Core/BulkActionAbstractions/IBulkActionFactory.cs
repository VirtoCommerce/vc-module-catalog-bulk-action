namespace VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionAbstractions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;

    public interface IBulkActionFactory
    {
        IBulkAction Create(BulkActionContext context);
    }
}