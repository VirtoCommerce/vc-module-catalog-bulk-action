namespace VirtoCommerce.CatalogBulkActionsModule.Web
{
    using System.Web.Http;

    using Microsoft.Practices.Unity;

    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionAbstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionImplementations;
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;
    using VirtoCommerce.CatalogBulkActionsModule.Core.DataSourceAbstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogBulkActionsModule.Data.DataSources;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Extensions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Web.JsonConverters;
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

            // to shared module
            _container.RegisterInstance<IBulkActionRegistrar>(new BulkActionRegistrar());
            _container.RegisterType<IBulkActionExecutor, BulkActionExecutor>();

            // to custom module
            _container.RegisterType<ISearchService, SearchService>();
            _container.RegisterType<IMover<Category>, CategoryMover>();
            _container.RegisterType<IMover<CatalogProduct>, ProductMover>();
            _container.RegisterType<IBulkPropertyUpdateManager, BulkPropertyUpdateManager>();
            _container.RegisterType<IPagedDataSourceFactory, PagedDataSourceFactory>();
            _container.RegisterType<IBulkActionFactory, BulkActionFactory>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            var httpConfiguration = _container.Resolve<HttpConfiguration>();
            var converter = new BulkActionContextJsonConverter();
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(converter);

            AbstractTypeFactory<BulkActionContext>.RegisterType<CategoryChangeBulkActionContext>();
            AbstractTypeFactory<BulkActionContext>.RegisterType<PropertiesUpdateBulkActionContext>();

            // TechDebt: IItemService and similar does not decorated with vc-module-cache/CatalogServicesDecorator as it is not registered yet.
            // Cache decorator registration is in PostInitialize for all used service being init.
            // Thus items cache is not invalidated after the changes.
            // So need to handle this situation here. Possible solutions:
            // 1. WithActionFactory and WithDataSourceFactory should use registered creation factory (e.g. Func<IBulkUpdateActionFactory>) for deferred factories instantiation (IMHO preferred)
            // 2. Pass DI container (IUnityContainer) to the factories. (not safe because of potential harmful container usage there)
            // Workaround - turn off Smart caching in platform UI in Settings/Cache/General.
            RegisterBulkAction(nameof(CategoryChangeBulkAction), nameof(CategoryChangeBulkActionContext));
            RegisterBulkAction(nameof(PropertiesUpdateBulkAction), nameof(PropertiesUpdateBulkActionContext));
        }

        public override void SetupDatabase()
        {
            // idle
        }

        private void RegisterBulkAction(string name, string contextTypeName)
        {
            var actionDefinition = new BulkActionDefinition
                                       {
                                           Name = name,
                                           ApplicableTypes = new[] { nameof(CatalogProduct) },
                                           ContextTypeName = contextTypeName
                                       };
            var actionFactory = _container.Resolve<IBulkActionFactory>();
            var dataSourceFactory = _container.Resolve<IPagedDataSourceFactory>();
            var actionRegistrar = _container.Resolve<IBulkActionRegistrar>();

            var actionDefinitionBuilder = new BulkActionDefinitionBuilder(actionDefinition);
            var withActionFactory = actionDefinitionBuilder.WithActionFactory(actionFactory);
            var withDataSourceFactory = withActionFactory.WithDataSourceFactory(dataSourceFactory);

            actionRegistrar.Register(withDataSourceFactory);
        }
    }
}