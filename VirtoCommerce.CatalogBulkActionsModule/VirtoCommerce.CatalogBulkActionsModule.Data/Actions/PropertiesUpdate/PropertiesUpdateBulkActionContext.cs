namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using VirtoCommerce.CatalogModule.Web.Model;

    public class PropertiesUpdateBulkActionContext : BaseBulkActionContext
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public Property[] Properties { get; set; }
    }
}