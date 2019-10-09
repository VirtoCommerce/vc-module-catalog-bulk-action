namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using VirtoCommerce.Domain.Catalog.Model;

    using moduleModels = VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public interface IBulkUpdateActionPropertyManager
    {
        Property[] GetProperties(UpdatePropertiesBulkActionContext context);

        UpdatePropertiesBulkActionResult UpdateProperties(CatalogProduct[] products, moduleModels.Property[] propertiesToSet);
    }
}