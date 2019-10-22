namespace VirtoCommerce.CatalogBulkActionsModule.Core.Converters
{
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public static class PropertyConverter
    {
        public static Property ToWebModel(this VC.Property property)
        {
            var result = new Property
                             {
                                 Id = property.Id,
                                 Name = property.Name,
                                 Required = property.Required,
                                 Type = property.Type,
                                 Multivalue = property.Multivalue,
                                 CatalogId = property.CatalogId,
                                 CategoryId = property.CategoryId,
                                 Dictionary = property.Dictionary,
                                 ValueType = property.ValueType
                             };
            result.Type = property.Type;
            result.Multilanguage = property.Multilanguage;
            result.IsInherited = property.IsInherited;
            result.Hidden = property.Hidden;
            result.ValueType = property.ValueType;
            result.Type = property.Type;
            result.Attributes = property.Attributes?.Select(x => x.ToWebModel()).ToList();
            result.DisplayNames = property.DisplayNames;
            result.ValidationRule = property.ValidationRules?.FirstOrDefault()?.ToWebModel();

            return result;
        }
    }
}