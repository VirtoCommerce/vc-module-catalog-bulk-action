namespace VirtoCommerce.CatalogBulkActionsModule.Data.Extensions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionAbstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionImplementations;
    using VirtoCommerce.CatalogBulkActionsModule.Core.DataSourceAbstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;

    public static class DefinitionBuilderExtensions
    {
        public static BulkActionDefinitionBuilder WithActionFactory(
            this BulkActionDefinitionBuilder builder,
            IBulkActionFactory factory)
        {
            builder.BulkActionDefinition.Factory = factory;
            return builder;
        }

        public static BulkActionDefinitionBuilder WithDataSourceFactory(
            this BulkActionDefinitionBuilder builder,
            IPagedDataSourceFactory factory)
        {
            builder.BulkActionDefinition.DataSourceFactory = factory;
            return builder;
        }
    }
}