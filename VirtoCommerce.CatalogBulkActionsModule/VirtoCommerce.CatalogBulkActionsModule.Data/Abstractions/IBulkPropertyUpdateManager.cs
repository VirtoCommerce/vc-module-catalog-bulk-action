namespace VirtoCommerce.CatalogBulkActionsModule.Data.Abstractions
{
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.Domain.Catalog.Model;

    using moduleModels = VirtoCommerce.BulkActionsModule.Core.Models;

    public interface IBulkPropertyUpdateManager
    {
        Property[] GetProperties(PropertiesUpdateBulkActionContext context);

        BulkActionResult UpdateProperties(CatalogProduct[] products, moduleModels.Property[] properties);
    }
}