namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    /// <summary>
    /// Image asset
    /// </summary>
    public class Image : AssetBase
    {
        public Image()
        {
            TypeId = "image";
            Group = "images";
        }

        public int SortOrder { get; set; }
    }
}