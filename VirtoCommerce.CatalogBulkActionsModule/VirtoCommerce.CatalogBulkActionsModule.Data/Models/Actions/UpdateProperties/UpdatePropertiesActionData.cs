namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public class UpdatePropertiesActionData : IBulkUpdateActionData
    {
        public Property[] Properties { get; set; }
    }
}