namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public class CategoryMover : IMover<VC.Category>
    {
        private readonly ILazyServiceProvider _lazyServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryMover"/> class.
        /// </summary>
        /// <param name="lazyServiceProvider">
        /// The service provider.
        /// </param>
        public CategoryMover(ILazyServiceProvider lazyServiceProvider)
        {
            _lazyServiceProvider = lazyServiceProvider;
        }

        public void Confirm(IEnumerable<VC.Category> entities)
        {
            var categoryService = _lazyServiceProvider.Resolve<ICategoryService>();
            categoryService.Update(entities.ToArray());
        }

        public List<VC.Category> Prepare(MoveOperationContext moveOperationContext)
        {
            var result = new List<VC.Category>();
            var categoryService = _lazyServiceProvider.Resolve<ICategoryService>();

            foreach (var listEntryCategory in moveOperationContext.Entries.Where(
                entry => entry.Type.EqualsInvariant(ListEntryCategory.TypeName)))
            {
                var category = categoryService.GetById(listEntryCategory.Id, VC.CategoryResponseGroup.Info);
                var targetCategory = categoryService.GetById(
                    moveOperationContext.Category,
                    VC.CategoryResponseGroup.WithOutlines);

                if (category.Id == moveOperationContext.Category)
                {
                    throw new ArgumentException("Unable to move category to itself");
                }

                var ids = targetCategory.Outlines.SelectMany(outline => outline.Items).Select(outline => outline.Id);

                if (ids.Any(outline => outline.EqualsInvariant(category.Id)))
                {
                    throw new ArgumentException("Unable to move category to its descendant");
                }

                if (category.CatalogId != moveOperationContext.Catalog)
                {
                    category.CatalogId = moveOperationContext.Catalog;
                }

                if (category.ParentId != moveOperationContext.Category)
                {
                    category.ParentId = moveOperationContext.Category;
                }

                result.Add(category);
            }

            return result;
        }
    }
}