namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public class UpdatePropertiesActionContext : BulkUpdateActionContext
    {
        public ListEntryDataQuery DataQuery { get; set; }

        public Property[] Properties { get; set; }
    }
}