namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.PropertiesUpdate
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public class PropertiesUpdateBulkActionContext : BulkActionContext
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public Property[] Properties { get; set; }
    }
}