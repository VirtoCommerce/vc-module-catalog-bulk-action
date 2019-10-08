namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.ChangeCategory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using VC = VirtoCommerce.Domain.Catalog.Model;

    public class ChangeCategoryBulkUpdateAction : IBulkUpdateAction
    {
        private readonly ICatalogService _catalogService;

        private readonly IListEntryMover<VC.Category> _categoryMover;

        private readonly ChangeCategoryActionContext _context;

        private readonly IListEntryMover<VC.CatalogProduct> _productMover;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeCategoryBulkUpdateAction"/> class.
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
        public ChangeCategoryBulkUpdateAction(
            ICatalogService catalogService,
            IListEntryMover<VC.Category> categoryMover,
            IListEntryMover<VC.CatalogProduct> productMover,
            ChangeCategoryActionContext context)
        {
            _catalogService = catalogService;
            _categoryMover = categoryMover;
            _productMover = productMover;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BulkUpdateActionContext Context => _context;

        public BulkUpdateActionResult Execute(IEnumerable<IEntity> entities)
        {
            var listEntries = entities.Cast<ListEntry>().ToArray();
            var result = BulkUpdateActionResult.Success;
            var moveInfo = new MoveOperationContext
                               {
                                   Catalog = _context.CatalogId,
                                   Category = _context.CategoryId,
                                   ListEntries = listEntries,
                               };

            var categories = _categoryMover.PrepareMove(moveInfo);
            var products = _productMover.PrepareMove(moveInfo);

            _categoryMover.ConfirmMove(categories);
            _productMover.ConfirmMove(products);

            return result;
        }

        public IBulkUpdateActionData GetActionData()
        {
            return null;
        }

        public BulkUpdateActionResult Validate()
        {
            var result = BulkUpdateActionResult.Success;

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