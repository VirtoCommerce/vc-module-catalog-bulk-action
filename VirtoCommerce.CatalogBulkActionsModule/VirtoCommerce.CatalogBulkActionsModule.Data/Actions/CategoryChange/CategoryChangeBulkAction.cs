namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogModule.Web.Model;
    using VirtoCommerce.CatalogModule.Web.Services;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using Category = VirtoCommerce.Domain.Catalog.Model.Category;

    public class CategoryChangeBulkAction : IBulkAction
    {
        private readonly CategoryChangeBulkActionContext _context;

        private readonly ILazyServiceProvider _lazyServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryChangeBulkAction"/> class.
        /// </summary>
        /// <param name="lazyServiceProvider">
        /// The service provider.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public CategoryChangeBulkAction(
            ILazyServiceProvider lazyServiceProvider,
            CategoryChangeBulkActionContext context)
        {
            _lazyServiceProvider = lazyServiceProvider;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BulkActionContext Context => _context;

        public BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var entries = entities.Cast<ListEntry>().ToArray();
            var moveInfo = new MoveInfo
            {
                Catalog = _context.CatalogId,
                Category = _context.CategoryId,
                ListEntries = entries
            };

            ValidateMoveInfo(moveInfo);

            var categoryMover = _lazyServiceProvider.Resolve<ListEntryMover<Category>>();
            var productMover = _lazyServiceProvider.Resolve<ListEntryMover<CatalogProduct>>();

            var categories = categoryMover.PrepareMove(moveInfo);
            var products = productMover.PrepareMove(moveInfo);

            categoryMover.ConfirmMove(categories);
            productMover.ConfirmMove(products);

            return BulkActionResult.Success;
        }

        public object GetActionData()
        {
            return null;
        }

        public BulkActionResult Validate()
        {
            var result = BulkActionResult.Success;

            var catalogService = _lazyServiceProvider.Resolve<ICatalogService>();
            var dstCatalog = catalogService.GetById(_context.CatalogId);
            if (dstCatalog.IsVirtual)
            {
                result.Succeeded = false;
                result.Errors.Add("Unable to move in virtual catalog");
            }

            return result;
        }

        private static bool IsEqual(string a, string b)
        {
            if (a == b)
            {
                return true;
            }

            return false;
        }

        private static void ValidateMoveInfo(MoveInfo moveInfo)
        {
            var exception = new Exception("Cannot be moved to a subcategory or into the same category");
            foreach (var entry in moveInfo.ListEntries)
            {
                if (IsEqual(moveInfo.Category, entry.Id))
                {
                    throw exception;
                }
            }
        }
    }
}