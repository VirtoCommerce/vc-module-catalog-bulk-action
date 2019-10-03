namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.UpdateProperties
{
    using VirtoCommerce.Domain.Catalog.Model;

    using web = VirtoCommerce.CatalogBulkActionsModule.Core.Models;


    public interface IBulkUpdatePropertyManager
    {
        Property[] GetProperties(UpdatePropertiesActionContext context);
        UpdatePropertiesResult UpdateProperties(CatalogProduct[] products, web.Property[] propertiesToSet);
    }
}