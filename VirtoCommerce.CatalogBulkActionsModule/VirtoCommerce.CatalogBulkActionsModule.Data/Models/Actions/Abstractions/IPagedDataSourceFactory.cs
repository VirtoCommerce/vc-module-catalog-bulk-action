namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions
{
    public interface IPagedDataSourceFactory
    {
        IPagedDataSource Create(BulkActionContext context);
    }
}