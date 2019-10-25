namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.Domain.Catalog.Model;

    using CatalogModule = VirtoCommerce.CatalogModule.Web.Model;

    public interface IBulkPropertyUpdateManager
    {
        Property[] GetProperties(BulkActionContext context);

        BulkActionResult UpdateProperties(CatalogProduct[] products, CatalogModule.Property[] properties);
    }
}