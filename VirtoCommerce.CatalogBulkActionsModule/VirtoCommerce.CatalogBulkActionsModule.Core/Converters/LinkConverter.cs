namespace VirtoCommerce.CatalogBulkActionsModule.Core.Converters
{
    using Omu.ValueInjecter;
    using moduleModel = VirtoCommerce.Domain.Catalog.Model;
    using webModel = VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    public static class LinkConverter
    {
        public static webModel.CategoryLink ToWebModel(this moduleModel.CategoryLink link)
        {
            var retVal = new webModel.CategoryLink();

            retVal.CatalogId = link.CatalogId;
            retVal.CategoryId = link.CategoryId;
            retVal.Priority = link.Priority;

            return retVal;
        }

        public static moduleModel.CategoryLink ToCoreModel(this webModel.CategoryLink link)
        {
            var retVal = new moduleModel.CategoryLink();
            retVal.InjectFrom(link);
            return retVal;
        }
    }
}