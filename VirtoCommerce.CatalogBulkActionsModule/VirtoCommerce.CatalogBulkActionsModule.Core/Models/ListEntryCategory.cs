namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using System.Linq;

    using CatalogModule = VirtoCommerce.CatalogModule.Web.Model;

    /// <summary>
    /// Category ListEntry record.
    /// </summary>
    public class ListEntryCategory : ListEntry
    {
        public const string TypeName = "category";

        public ListEntryCategory(CatalogModule.Category category)
            : base(TypeName, category)
        {
            ImageUrl = category.ImgSrc;
            Code = category.Code;
            Name = category.Name;
            IsActive = category.IsActive;
            CatalogId = category.CatalogId;

            if (!string.IsNullOrEmpty(category.Outline))
            {
                Outline = category.Outline.Split('/').Select(segment => segment).ToArray();
            }

            if (!string.IsNullOrEmpty(category.Path))
            {
                Path = category.Path.Split('/').Select(segment => segment).ToArray();
            }

            if (category.Links != null)
            {
                Links = category.Links.Select(link => new CatalogModule.ListEntryLink(link)).ToArray();
            }
        }
    }
}