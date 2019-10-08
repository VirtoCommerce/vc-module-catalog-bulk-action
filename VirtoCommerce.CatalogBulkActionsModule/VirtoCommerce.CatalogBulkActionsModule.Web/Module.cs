namespace VirtoCommerce.CatalogBulkActionsModule.Web
{
    using System.Web.Http;

    using Microsoft.Practices.Unity;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Extensions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.ChangeCategory;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties;
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

            _container.RegisterType<IListEntrySearchService, ListEntrySearchService>();
            _container.RegisterType<IListEntryMover<Category>, CategoryMover>();
            _container.RegisterType<IListEntryMover<CatalogProduct>, ProductMover>();

            _container.RegisterInstance<IBulkUpdateActionRegistrar>(new BulkUpdateActionRegistrar());
            _container.RegisterType<IBulkUpdateActionExecutor, BulkUpdateActionExecutor>();
            _container.RegisterType<IBulkUpdateActionFactory, BulkUpdateActionFactory>();
            _container.RegisterType<IBulkUpdatePropertyManager, BulkUpdatePropertyManager>();
            _container.RegisterType<IPagedDataSourceFactory, BulkUpdateDataSourceFactory>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            var httpConfiguration = _container.Resolve<HttpConfiguration>();
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new BulkUpdateActionContextJsonConverter());

            AbstractTypeFactory<BulkUpdateActionContext>.RegisterType<ChangeCategoryActionContext>();
            AbstractTypeFactory<BulkUpdateActionContext>.RegisterType<UpdatePropertiesActionContext>();

            // TechDebt: IItemService and similar does not decorated with vc-module-cache/CatalogServicesDecorator as it is not registered yet.
            // Cache decorator registration is in PostInitialize for all used service being init.
            // Thus items cache is not invalidated after the changes.
            // So need to handle this situation here. Possible solutions:
            // 1. WithActionFactory and WithDataSourceFactory should use registered creation factory (e.g. Func<IBulkUpdateActionFactory>) for deferred factories instantiation (IMHO preferred)
            // 2. Pass DI container (IUnityContainer) to the factories. (not safe because of potential harmful container usage there)
            // Workaround - turn off Smart caching in platform UI in Settings/Cache/General.
            Register(nameof(ChangeCategoryBulkUpdateAction), nameof(ChangeCategoryActionContext));
            Register(nameof(UpdatePropertiesBulkUpdateAction), nameof(UpdatePropertiesActionContext));
        }

        public override void SetupDatabase()
        {
            // idle
        }

        private void Register(string name, string contextTypeName)
        {
            var actionDefinition = new BulkUpdateActionDefinition
                                       {
                                           Name = name,
                                           AppliableTypes = new[] { nameof(CatalogProduct) },
                                           ContextTypeName = contextTypeName
                                       };
            var actionFactory = _container.Resolve<IBulkUpdateActionFactory>();
            var dataSourceFactory = _container.Resolve<IPagedDataSourceFactory>();
            var actionRegistrar = _container.Resolve<IBulkUpdateActionRegistrar>();

            var actionDefinitionBuilder = new BulkUpdateActionDefinitionBuilder(actionDefinition);
            var withActionFactory = actionDefinitionBuilder.WithActionFactory(actionFactory);
            var withDataSourceFactory = withActionFactory.WithDataSourceFactory(dataSourceFactory);

            actionRegistrar.Register(withDataSourceFactory);
        }
    }
}