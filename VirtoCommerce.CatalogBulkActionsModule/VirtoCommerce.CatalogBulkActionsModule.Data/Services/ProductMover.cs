namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public class ProductMover : IMover<VC.CatalogProduct>
    {
        private readonly ILazyServiceProvider _lazyServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductMover"/> class.
        /// </summary>
        /// <param name="lazyServiceProvider">
        /// The service Provider.
        /// </param>
        public ProductMover(ILazyServiceProvider lazyServiceProvider)
        {
            _lazyServiceProvider = lazyServiceProvider;
        }

        public void Confirm(IEnumerable<VC.CatalogProduct> entities)
        {
            var itemService = _lazyServiceProvider.Resolve<IItemService>();
            itemService.Update(entities.ToArray());
        }

        public List<VC.CatalogProduct> Prepare(MoveOperationContext moveOperationContext)
        {
            var result = new List<VC.CatalogProduct>();
            var itemService = _lazyServiceProvider.Resolve<IItemService>();

            foreach (var listEntryProduct in moveOperationContext.Entries.Where(
                listEntry => listEntry.Type.EqualsInvariant(ListEntryProduct.TypeName)))
            {
                var product = itemService.GetById(listEntryProduct.Id, VC.ItemResponseGroup.ItemLarge);
                if (product.CatalogId == moveOperationContext.Catalog)
                {
                    // idle
                }
                else
                {
                    product.CatalogId = moveOperationContext.Catalog;
                    product.CategoryId = null;
                    foreach (var variation in product.Variations)
                    {
                        variation.CatalogId = moveOperationContext.Catalog;
                        variation.CategoryId = null;
                    }
                }

                if (product.CategoryId == moveOperationContext.Category)
                {
                    // idle
                }
                else
                {
                    product.CategoryId = moveOperationContext.Category;
                    foreach (var variation in product.Variations)
                    {
                        variation.CategoryId = moveOperationContext.Category;
                    }
                }

                result.Add(product);
                result.AddRange(product.Variations);
            }

            return result;
        }
    }
}