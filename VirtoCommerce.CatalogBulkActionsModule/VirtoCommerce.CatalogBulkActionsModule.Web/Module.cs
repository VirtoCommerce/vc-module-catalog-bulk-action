﻿namespace VirtoCommerce.CatalogBulkActionsModule.Web
{
    using Microsoft.Practices.Unity;

    using VirtoCommerce.BulkActionsModule.Core.BulkActionAbstractions;
    using VirtoCommerce.BulkActionsModule.Core.BulkActionModels;
    using VirtoCommerce.BulkActionsModule.Core.DataSourceAbstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Abstractions;
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
            _container.RegisterType<IPagedDataSourceFactory, PagedDataSourceFactory>();
            _container.RegisterType<IBulkActionFactory, BulkActionFactory>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

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
            var dataSourceFactory = _container.Resolve<IPagedDataSourceFactory>();
            var actionFactory = _container.Resolve<IBulkActionFactory>();
            var actionRegistrar = _container.Resolve<IBulkActionRegistrar>();

            var actionDefinition = new BulkActionDefinition(
                name,
                contextTypeName,
                new[] { nameof(CatalogProduct) },
                dataSourceFactory,
                actionFactory);
            actionRegistrar.Register(actionDefinition);
        }
    }
}