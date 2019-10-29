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

        private readonly ILazyServiceProvider _lazyLazyServiceProvider;

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
            _lazyLazyServiceProvider = lazyServiceProvider;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BulkActionContext Context => _context;

        public BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var entries = entities.Cast<ListEntry>().ToArray();
            var result = BulkActionResult.Success;
            var operationContext = new MoveContext
            {
                Catalog = _context.CatalogId,
                Category = _context.CategoryId,
                ListEntries = entries
            };

            var categoryMover = _lazyLazyServiceProvider.Resolve<ListEntryMover<Category>>();
            var productMover = _lazyLazyServiceProvider.Resolve<ListEntryMover<CatalogProduct>>();
            var categories = categoryMover.PrepareMove(operationContext);
            var products = productMover.PrepareMove(operationContext);

            categoryMover.ConfirmMove(categories);
            productMover.ConfirmMove(products);

            return result;
        }

        public object GetActionData()
        {
            return null;
        }

        public BulkActionResult Validate()
        {
            var result = BulkActionResult.Success;

            var catalogService = _lazyLazyServiceProvider.Resolve<ICatalogService>();
            var dstCatalog = catalogService.GetById(_context.CatalogId);
            if (dstCatalog.IsVirtual)
            {
                result.Succeeded = false;
                result.Errors.Add("Unable to move in virtual catalog");
            }

            return result;
        }
    }
}