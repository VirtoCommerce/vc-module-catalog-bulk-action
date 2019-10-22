﻿namespace VirtoCommerce.CatalogBulkActionsModule.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Commerce.Model;
    using VirtoCommerce.Platform.Core.Common;

    /// <summary>
    /// Merchandising item.
    /// </summary>
    public class Product : AuditableEntity, ISeoSupport, IHasOutlines
    {
        private string _imgSrc;

        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>
        /// The assets.
        /// </value>
        public ICollection<Asset> Assets { get; set; }

        /// <summary>
        /// Gets or sets the associations.
        /// </summary>
        /// <value>
        /// The associations.
        /// </value>
        public ICollection<ProductAssociation> Associations { get; set; }

        /// <summary>
        /// Gets or sets the catalog id that this product belongs to.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the category id that this product belongs to.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the download expiration. (for digital product only)
        /// </summary>
        /// <value>
        /// The download expiration.
        /// </value>
        public DateTime? DownloadExpiration { get; set; }

        /// <summary>
        /// Gets or sets the type of the download. (for digital product only)
        /// </summary>
        /// <value>
        /// The type of the download.
        /// </value>
        public string DownloadType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> can be reviewed in storefront.
        /// </summary>
        public bool? EnableReview { get; set; }

        /// <summary>
        /// Gets or sets a sale expiration date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Global Trade Item Number.
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> has user agreement. (for digital product only)
        /// </summary>
        public bool? HasUserAgreement { get; set; }

        /// <summary>
        /// Gets or sets the height. (for physical product only)
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public decimal? Height { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>
        /// The images.
        /// </value>
        public ICollection<Image> Images { get; set; }

        /// <summary>
        /// Gets the default image for the product.
        /// </summary>
        /// <value>
        /// The image source URL.
        /// </value>
        public string ImgSrc
        {
            get
            {
                if (_imgSrc != null)
                {
                    return _imgSrc;
                }

                if (Images != null && Images.Any())
                {
                    _imgSrc = Images.First().Url;
                }

                return _imgSrc;
            }
        }

        /// <summary>
        /// Gets or sets the date and time that this product was last indexed at.
        /// </summary>
        /// <value>
        /// The indexing date.
        /// </value>
        public DateTime? IndexingDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> is active.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> is buyable.
        /// </summary>
        public bool? IsBuyable { get; set; }

        /// <summary>
        /// Gets or sets the length. (for physical product only)
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public decimal? Length { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public ICollection<CategoryLink> Links { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer part number for this product.
        /// </summary>
        /// <value>
        /// The manufacturer part number.
        /// </value>
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of download. (for digital product only)
        /// </summary>
        /// <value>
        /// The maximum number of download.
        /// </value>
        public int? MaxNumberOfDownload { get; set; }

        /// <summary>
        /// Gets or sets the maximum quantity of the product that a customer can buy.
        /// </summary>
        /// <value>
        /// The maximum quantity.
        /// </value>
        public int? MaxQuantity { get; set; }

        /// <summary>
        /// Gets or sets the dimensions measure unit. (for physical product only)
        /// </summary>
        /// <value>
        /// The measure unit.
        /// </value>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the minimum quantity of the product that a customer can buy.
        /// </summary>
        /// <value>
        /// The minimum quantity.
        /// </value>
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a product outline in physical catalog (all parent categories ids concatenated. E.g. (1/21/344))
        /// </summary>
        public string Outline { get; set; }

        public ICollection<Outline> Outlines { get; set; }

        /// <summary>
        /// Gets or sets predefined length dimension package type.
        /// </summary>
        public string PackageType { get; set; }

        /// <summary>
        /// Gets or sets the product path in physical catalog.
        /// (all parent categories names concatenated. E.g. (parent1/parent2))
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a product order position physical catalog.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the type of the product. (can be Physical, Digital or Subscription).
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public ICollection<Property> Properties { get; set; }

        /// <summary>
        /// Gets or sets the referenced associations.
        /// </summary>
        public ICollection<ProductAssociation> ReferencedAssociations { get; set; }

        /// <summary>
        /// Gets or sets the reviews.
        /// </summary>
        /// <value>
        /// The reviews.
        /// </value>
        public ICollection<EditorialReview> Reviews { get; set; }

        /// <summary>
        /// Gets or sets the security scopes.
        /// </summary>
        public string[] SecurityScopes { get; set; }

        /// <summary>
        /// Gets or sets the list of SEO information records.
        /// </summary>
        /// <value>
        /// The seo infos.
        /// </value>
        public ICollection<SeoInfo> SeoInfos { get; set; }

        /// <summary>
        /// Gets or sets the seo object type.
        /// </summary>
        public string SeoObjectType => GetType().Name;

        /// <summary>
        /// Gets or sets the type of the shipping.
        /// </summary>
        /// <value>
        /// The type of the shipping.
        /// </value>
        public string ShippingType { get; set; }

        /// <summary>
        /// Gets or sets the sale start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the tax.
        /// </summary>
        /// <value>
        /// The type of the tax.
        /// </value>
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the titular item id for a variation.
        /// </summary>
        /// <value>
        /// The titular item identifier.
        /// </value>
        public string TitularItemId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> inventory is tracked.
        /// </summary>
        public bool? TrackInventory { get; set; }

        /// <summary>
        /// Gets or sets the variations.
        /// </summary>
        /// <value>
        /// The variations.
        /// </value>
        public ICollection<Product> Variations { get; set; }

        /// <summary>
        /// Gets or sets the product vendor.
        /// </summary>
        /// <value>
        /// The vendor.
        /// </value>
        public string Vendor { get; set; }

        /// <summary>
        /// Gets or sets the weight. (for physical product only)
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets the weight unit. (for physical product only)
        /// </summary>
        /// <value>
        /// The weight unit.
        /// </value>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the width. (for physical product only)
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public decimal? Width { get; set; }

        public virtual Product FromModel(CatalogProduct product)
        {
            return this;
        }

        public virtual void ReduceDetails(string responseGroup)
        {
            var productResponseGroup = EnumUtility.SafeParseFlags(responseGroup, ItemResponseGroup.ItemLarge);

            if (!productResponseGroup.HasFlag(ItemResponseGroup.ItemAssets))
            {
                Assets = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.ItemProperties))
            {
                Properties = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.ItemAssociations))
            {
                Associations = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.ReferencedAssociations))
            {
                ReferencedAssociations = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.ItemEditorialReviews))
            {
                Reviews = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.Links))
            {
                Links = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.Outlines))
            {
                Outlines = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.Seo))
            {
                SeoInfos = null;
            }

            if (!productResponseGroup.HasFlag(ItemResponseGroup.Variations))
            {
                Variations = null;
            }
        }

        public virtual CatalogProduct ToModel(CatalogProduct product)
        {
            return product;
        }
    }
}