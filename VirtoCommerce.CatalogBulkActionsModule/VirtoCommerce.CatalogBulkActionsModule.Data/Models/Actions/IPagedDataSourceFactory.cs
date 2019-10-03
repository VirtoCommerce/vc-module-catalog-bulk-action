namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models.Actions;

    public interface IPagedDataSourceFactory
    {
        IPagedDataSource Create(BulkUpdateActionContext context);
    }
}