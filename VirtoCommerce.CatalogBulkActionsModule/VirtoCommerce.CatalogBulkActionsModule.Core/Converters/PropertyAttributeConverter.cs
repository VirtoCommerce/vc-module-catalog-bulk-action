namespace VirtoCommerce.CatalogBulkActionsModule.Core.Converters
{
    using Omu.ValueInjecter;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public static class PropertyAttributeConverter
    {
        public static VC.PropertyAttribute ToCoreModel(this PropertyAttribute attribute)
        {
            var result = new VC.PropertyAttribute();
            result.InjectFrom(attribute);
            return result;
        }

        public static PropertyAttribute ToWebModel(this VC.PropertyAttribute attribute)
        {
            var result = new PropertyAttribute { Id = attribute.Id, Name = attribute.Name, Value = attribute.Value };
            return result;
        }
    }
}