namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using VirtoCommerce.Platform.Core.Common;

    public class PropertyValidationRule : Entity
    {
        public bool IsUnique { get; set; }

        public int? CharCountMin { get; set; }

        public int? CharCountMax { get; set; }

        public string RegExp { get; set; }
    }
}