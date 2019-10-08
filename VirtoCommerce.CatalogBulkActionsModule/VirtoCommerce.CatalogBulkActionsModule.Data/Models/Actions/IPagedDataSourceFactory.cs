namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    public interface IPagedDataSourceFactory
    {
        IPagedDataSource Create(BulkUpdateActionContext context);
    }
}