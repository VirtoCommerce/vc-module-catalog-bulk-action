namespace VirtoCommerce.CatalogBulkActionsModule.Core.Converters
{
    using Omu.ValueInjecter;
    using moduleModel = VirtoCommerce.Domain.Catalog.Model;
    using webModel = VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public static class PropertyValueConverter
    {
        public static webModel.PropertyValue ToWebModel(this moduleModel.PropertyValue propValue)
        {
            var retVal = new webModel.PropertyValue();

            retVal.Id = propValue.Id;
            retVal.LanguageCode = propValue.LanguageCode;
            retVal.PropertyId = propValue.PropertyId;
            retVal.PropertyName = propValue.PropertyName;
            retVal.ValueId = propValue.ValueId;
            retVal.ValueType = propValue.ValueType;
            retVal.Alias = propValue.Alias;
            retVal.IsInherited = propValue.IsInherited;

            if (propValue.Property != null)
            {
                retVal.PropertyId = propValue.Property.Id;
                retVal.PropertyMultivalue = propValue.Property.Multivalue;
            }
            retVal.Value = propValue.Value;

            return retVal;
        }

        public static moduleModel.PropertyValue ToCoreModel(this webModel.PropertyValue propValue)
        {
            var retVal = new moduleModel.PropertyValue();
            retVal.InjectFrom(propValue);
            retVal.Value = propValue.Value;
            retVal.ValueType = (moduleModel.PropertyValueType)(int)propValue.ValueType;
            return retVal;
        }
    }
}