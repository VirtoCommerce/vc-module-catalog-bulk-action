namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using VirtoCommerce.BulkActionsModule.Core.Models;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;

    public class PropertiesUpdateBulkActionContext : BulkActionContext
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public Property[] Properties { get; set; }
    }
}