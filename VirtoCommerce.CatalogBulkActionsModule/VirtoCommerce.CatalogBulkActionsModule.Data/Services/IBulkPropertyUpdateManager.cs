namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.Domain.Catalog.Model;

    using moduleModels = VirtoCommerce.BulkActionsModule.Core.Models;
    using Property = VirtoCommerce.Domain.Catalog.Model.Property;

    public interface IBulkPropertyUpdateManager
    {
        Property[] GetProperties(BulkActionContext context);

        BulkActionResult UpdateProperties(CatalogProduct[] products, moduleModels.Property[] properties);
    }
}