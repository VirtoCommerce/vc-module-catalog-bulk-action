using VirtoCommerce.CatalogBulkActionsModule.Core.Services;

namespace VirtoCommerce.CatalogBulkActionsModule.Web
{
    using Microsoft.Practices.Unity;

    using VirtoCommerce.CatalogBulkActionsModule.Core;

    public class LazyServiceProvider : ILazyServiceProvider
    {
        private readonly IUnityContainer _container;

        public LazyServiceProvider(IUnityContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
