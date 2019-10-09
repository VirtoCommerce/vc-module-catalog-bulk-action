namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public class UpdatePropertiesBulkActionContext : BulkActionContext
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public Property[] Properties { get; set; }
    }
}