﻿namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.CategoryChange
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public class CategoryChangeBulkAction : IBulkAction
    {
        private readonly ICatalogService _catalogService;

        private readonly IMover<VC.Category> _categoryMover;

        private readonly CategoryChangeBulkActionContext _context;

        private readonly IMover<VC.CatalogProduct> _productMover;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryChangeBulkAction"/> class.
        /// </summary>
        /// <param name="catalogService">
        /// The catalog service.
        /// </param>
        /// <param name="categoryMover">
        /// The category mover.
        /// </param>
        /// <param name="productMover">
        /// The product mover.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public CategoryChangeBulkAction(
            ICatalogService catalogService,
            IMover<VC.Category> categoryMover,
            IMover<VC.CatalogProduct> productMover,
            CategoryChangeBulkActionContext context)
        {
            _catalogService = catalogService;
            _categoryMover = categoryMover;
            _productMover = productMover;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BulkActionContext Context => _context;

        public BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var listEntries = entities.Cast<ListEntry>().ToArray();
            var result = BulkActionResult.Success;
            var moveInfo = new MoveOperationContext
                               {
                                   Catalog = _context.CatalogId,
                                   Category = _context.CategoryId,
                                   Entries = listEntries,
                               };

            var categories = _categoryMover.Prepare(moveInfo);
            var products = _productMover.Prepare(moveInfo);

            _categoryMover.Confirm(categories);
            _productMover.Confirm(products);

            return result;
        }

        public object GetActionData()
        {
            return null;
        }

        public BulkActionResult Validate()
        {
            var result = BulkActionResult.Success;

            var dstCatalog = _catalogService.GetById(_context.CatalogId);
            if (dstCatalog.IsVirtual)
            {
                result.Succeeded = false;
                result.Errors.Add("Unable to move in virtual catalog");
            }

            return result;
        }
    }
}