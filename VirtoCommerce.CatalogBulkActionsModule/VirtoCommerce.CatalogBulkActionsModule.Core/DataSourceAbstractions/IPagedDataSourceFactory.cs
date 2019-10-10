namespace VirtoCommerce.CatalogBulkActionsModule.Core.DataSourceAbstractions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;

    public interface IPagedDataSourceFactory
    {
        IPagedDataSource Create(BulkActionContext context);
    }
}