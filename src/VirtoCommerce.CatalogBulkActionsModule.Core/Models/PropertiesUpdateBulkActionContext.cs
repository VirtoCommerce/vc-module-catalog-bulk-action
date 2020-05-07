using VirtoCommerce.CatalogModule.Core.Model;

namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    public class PropertiesUpdateBulkActionContext : BaseBulkActionContext
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public Property[] Properties { get; set; }
    }
}
