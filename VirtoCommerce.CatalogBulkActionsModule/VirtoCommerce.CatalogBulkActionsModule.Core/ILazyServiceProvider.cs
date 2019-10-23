namespace VirtoCommerce.CatalogBulkActionsModule.Core
{
    public interface ILazyServiceProvider
    {
        T Resolve<T>();
    }
}