using VirtoCommerce.CatalogModule.Core.Model;

namespace VirtoCommerce.CatalogBulkActionsModule.Core.Services
{
    public interface IBulkPropertyUpdateManager
    {
        Property[] GetProperties(BulkActionContext context);

        BulkActionResult UpdateProperties(CatalogProduct[] products, Property[] properties);
    }
}
