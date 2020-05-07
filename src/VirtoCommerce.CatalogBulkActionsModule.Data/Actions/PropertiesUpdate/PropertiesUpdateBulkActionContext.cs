namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using CatalogModule = VirtoCommerce.CatalogModule.Web.Model;

    public class PropertiesUpdateBulkActionContext : BaseBulkActionContext
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public CatalogModule.Property[] Properties { get; set; }
    }
}