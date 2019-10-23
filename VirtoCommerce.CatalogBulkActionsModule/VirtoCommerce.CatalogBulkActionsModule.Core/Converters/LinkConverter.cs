namespace VirtoCommerce.CatalogBulkActionsModule.Core.Converters
{
    using Omu.ValueInjecter;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public static class LinkConverter
    {
        public static VC.CategoryLink ToCoreModel(this CategoryLink link)
        {
            var result = new VC.CategoryLink();
            result.InjectFrom(link);
            return result;
        }

        public static CategoryLink ToWebModel(this VC.CategoryLink link)
        {
            var result = new CategoryLink
            {
                CatalogId = link.CatalogId,
                CategoryId = link.CategoryId,
                Priority = link.Priority
            };

            return result;
        }
    }
}