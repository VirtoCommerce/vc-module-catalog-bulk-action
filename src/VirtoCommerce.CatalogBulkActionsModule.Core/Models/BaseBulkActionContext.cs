using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;

namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    public class BaseBulkActionContext : BulkActionContext
    {
        public DataQuery DataQuery { get; set; }
    }
}
