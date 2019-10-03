﻿namespace VirtoCommerce.CatalogBulkActionsModule.Core.Converters
{
    using moduleModel = VirtoCommerce.Domain.Catalog.Model;
    using webModel = VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public static class PropertyValidationRuleConverter
    {
        public static webModel.PropertyValidationRule ToWebModel(this moduleModel.PropertyValidationRule validationRule)
        {
            var retVal = new webModel.PropertyValidationRule();

            retVal.Id = validationRule.Id;
            retVal.IsUnique = validationRule.IsUnique;
            retVal.CharCountMin = validationRule.CharCountMin;
            retVal.CharCountMax = validationRule.CharCountMax;
            retVal.RegExp = validationRule.RegExp;

            return retVal;
        }

        public static moduleModel.PropertyValidationRule ToCoreModel(this webModel.PropertyValidationRule validationRule)
        {
            var retVal = new moduleModel.PropertyValidationRule();

            retVal.Id = validationRule.Id;
            retVal.IsUnique = validationRule.IsUnique;
            retVal.CharCountMin = validationRule.CharCountMin;
            retVal.CharCountMax = validationRule.CharCountMax;
            retVal.RegExp = validationRule.RegExp;

            return retVal;
        }
    }
}