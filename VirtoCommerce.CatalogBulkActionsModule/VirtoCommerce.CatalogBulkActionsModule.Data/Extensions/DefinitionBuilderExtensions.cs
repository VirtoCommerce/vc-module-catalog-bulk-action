namespace VirtoCommerce.CatalogBulkActionsModule.Data.Extensions
{
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;
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