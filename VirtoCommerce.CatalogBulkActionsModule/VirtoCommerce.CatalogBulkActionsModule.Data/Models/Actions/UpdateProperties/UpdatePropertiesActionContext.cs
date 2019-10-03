namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public class UpdatePropertiesActionContext : BulkUpdateActionContext
    {
        public Property[] Properties { get; set; }
        public ListEntryDataQuery DataQuery { get; set; }
    }
}