namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using VirtoCommerce.Domain.Catalog.Model;

    public class SearchCriteria
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCriteria"/> class.
        /// </summary>
        public SearchCriteria()
        {
            Take = 20;
        }

        /// <summary>
        /// Gets or sets the catalog ids.
        /// </summary>
        public string[] CatalogIds { get; set; }

        /// <summary>
        /// Gets or sets the category ids.
        /// </summary>
        public string[] CategoryIds { get; set; }

        /// <summary>
        /// Gets or sets the facets collection
        /// Item format: name:value1,value2,value3
        /// </summary>
        public string[] Facets { get; set; }

        /// <summary>
        /// Gets or sets the pricelist ids.
        /// </summary>
        public string[] PricelistIds { get; set; }

        /// <summary>
        /// Gets or sets the product types.
        /// Search product with specified types.
        /// </summary>
        public string[] ProductTypes { get; set; }

        /// <summary>
        /// Gets or sets the property values.
        /// For filtration by specified properties values.
        /// </summary>
        public PropertyValue[] PropertyValues { get; set; }

        /// <summary>
        /// Gets or sets the response group.
        /// </summary>
        public SearchResponseGroup ResponseGroup { get; set; }

        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets search terms collection
        /// Item format: name:value1,value2,value3
        /// </summary>
        public string[] Terms { get; set; }

        /// <summary>
        /// Gets or sets the vendor ids.
        /// </summary>
        public string[] VendorIds { get; set; }
    }
}