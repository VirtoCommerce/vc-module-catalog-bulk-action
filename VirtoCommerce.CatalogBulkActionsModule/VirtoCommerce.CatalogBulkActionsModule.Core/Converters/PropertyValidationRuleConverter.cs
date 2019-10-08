namespace VirtoCommerce.CatalogBulkActionsModule.Core.Converters
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public static class PropertyValidationRuleConverter
    {
        public static PropertyValidationRule ToWebModel(this VC.PropertyValidationRule validationRule)
        {
            var result = new PropertyValidationRule
                             {
                                 Id = validationRule.Id,
                                 IsUnique = validationRule.IsUnique,
                                 CharCountMin = validationRule.CharCountMin,
                                 CharCountMax = validationRule.CharCountMax,
                                 RegExp = validationRule.RegExp
                             };

            return result;
        }
    }
}