namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.PropertiesUpdate
{
    using VirtoCommerce.Domain.Catalog.Model;

    using moduleModels = VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using Property = VirtoCommerce.Domain.Catalog.Model.Property;

    public interface IBulkPropertyUpdateManager
    {
        Property[] GetProperties(PropertiesUpdateBulkActionContext context);

        PropertiesUpdateBulkActionResult UpdateProperties(CatalogProduct[] products, moduleModels.Property[] propertiesToSet);
    }
}