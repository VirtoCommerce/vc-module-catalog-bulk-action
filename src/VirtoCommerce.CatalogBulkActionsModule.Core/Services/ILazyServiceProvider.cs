namespace VirtoCommerce.CatalogBulkActionsModule.Core.Services
{
    public interface ILazyServiceProvider
    {
        T Resolve<T>();
    }
}
