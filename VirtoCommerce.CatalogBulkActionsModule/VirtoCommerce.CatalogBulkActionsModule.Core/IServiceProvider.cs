namespace VirtoCommerce.CatalogBulkActionsModule.Core
{
    public interface IServiceProvider
    {
        T Resolve<T>();
    }
}