namespace VirtoCommerce.CatalogBulkActionsModule.Web
{
    using Microsoft.Practices.Unity;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogBulkActionsModule.Data.DataSources;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Platform.Core.Common;
    using VirtoCommerce.Platform.Core.Modularity;

    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            base.Initialize();

            _container.RegisterType<ISearchService, SearchService>();
            _container.RegisterType<IMover<Category>, CategoryMover>();
            _container.RegisterType<IMover<CatalogProduct>, ProductMover>();
            _container.RegisterType<IBulkPropertyUpdateManager, BulkPropertyUpdateManager>();
            _container.RegisterType<IDataSourceFactory, DataSourceFactory>();
            _container.RegisterType<IBulkActionFactory, BulkActionFactory>();

            // This registration is necessary to avoid problems with caching.
            // These problems might occur when we doing the bulk operation and after completion, we don't see any result.
            // We think it's because we using the cache decorators here:
            // https://github.com/VirtoCommerce/vc-module-cache/blob/b6cb9ae85d1f38ff23149dffac806ce150896bb2/VirtoCommerce.CacheModule.Web/Module.cs#L56
            // Decorators dependencies might be registered in a container after this module will have registered their own and eventually, we will get wrong dependencies.
            // That's why we trying to resolve these problems with "lazy" dependency resolving.
            _container.RegisterInstance<ILazyServiceProvider>(new LazyServiceProvider(_container));
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            AbstractTypeFactory<BulkActionContext>.RegisterType<CategoryChangeBulkActionContext>();
            AbstractTypeFactory<BulkActionContext>.RegisterType<PropertiesUpdateBulkActionContext>();

            RegisterBulkAction(nameof(CategoryChangeBulkAction), nameof(CategoryChangeBulkActionContext));
            RegisterBulkAction(nameof(PropertiesUpdateBulkAction), nameof(PropertiesUpdateBulkActionContext));
        }

        private void RegisterBulkAction(string name, string contextTypeName)
        {
            var dataSourceFactory = _container.Resolve<IDataSourceFactory>();
            var actionFactory = _container.Resolve<IBulkActionFactory>();
            var actionDefinition = new BulkActionProvider(
                name,
                contextTypeName,
                new[] { nameof(CatalogProduct) },
                dataSourceFactory,
                actionFactory);

            var bulkActionDefinitionStorage = _container.Resolve<IBulkActionProviderStorage>();
            bulkActionDefinitionStorage.Add(actionDefinition);
        }
    }
}