namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using VirtoCommerce.Platform.Core.Common;

    /// <summary>
    /// Additional metainformation for a Property
    /// </summary>
    public class PropertyAttribute : Entity
    {
        public Property Property { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}