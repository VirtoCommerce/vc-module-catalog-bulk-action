namespace VirtoCommerce.CatalogBulkActionsModule.Core.Extensions
{
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;

    public static class BulkUpdateActionDefinitionBuilderExtensions
    {
        public static BulkUpdateActionDefinitionBuilder WithDataSourceFactory(this BulkUpdateActionDefinitionBuilder builder, IPagedDataSourceFactory factory)
        {
            builder.BulkUpdateActionDefinition.DataSourceFactory = factory;
            return builder;
        }

        public static BulkUpdateActionDefinitionBuilder WithActionFactory(this BulkUpdateActionDefinitionBuilder builder, IBulkUpdateActionFactory factory)
        {
            builder.BulkUpdateActionDefinition.Factory = factory;
            return builder;
        }

    }
}