namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;
    using VirtoCommerce.Domain.Catalog.Model;

    using moduleModels = VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public interface IBulkPropertyUpdateManager
    {
        Property[] GetProperties(PropertiesUpdateBulkActionContext context);

        BulkActionResult UpdateProperties(
            CatalogProduct[] products,
            moduleModels.Property[] properties);
    }
}