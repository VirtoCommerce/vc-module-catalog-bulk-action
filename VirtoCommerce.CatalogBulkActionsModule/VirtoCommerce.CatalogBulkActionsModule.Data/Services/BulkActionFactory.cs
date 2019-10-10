﻿namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.PropertiesUpdate;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;

    public class BulkActionFactory : IBulkActionFactory
    {
        private readonly IBulkPropertyUpdateManager bulkPropertyUpdateManager;

        private readonly ICatalogService _catalogService;

        private readonly IMover<Category> _categoryMover;

        private readonly ICategoryService _categoryService;

        private readonly IItemService _itemService;

        private readonly IMover<CatalogProduct> _productMover;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkActionFactory"/> class.
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
        /// <param name="bulkPropertyUpdateManager">
        /// The bulk update property manager.
        /// </param>
        /// <param name="itemService">
        /// The item service.
        /// </param>
        /// <param name="categoryService">
        /// The category service.
        /// </param>
        public BulkActionFactory(
            ICatalogService catalogService,
            IMover<Category> categoryMover,
            IMover<CatalogProduct> productMover,
            IBulkPropertyUpdateManager bulkPropertyUpdateManager,
            IItemService itemService,
            ICategoryService categoryService)
        {
            _catalogService = catalogService;
            _categoryMover = categoryMover;
            _productMover = productMover;
            this.bulkPropertyUpdateManager = bulkPropertyUpdateManager;
            _itemService = itemService;
            _categoryService = categoryService;
        }

        public IBulkAction Create(BulkActionContext context)
        {
            IBulkAction result = null;

            switch (context)
            {
                case CategoryChangeBulkActionContext changeCategoryActionContext:
                    result = new CategoryChangeBulkAction(
                        _catalogService,
                        _categoryMover,
                        _productMover,
                        changeCategoryActionContext);
                    break;

                case PropertiesUpdateBulkActionContext updatePropertiesActionContext:
                    result = new PropertiesUpdateBulkAction(
                        bulkPropertyUpdateManager,
                        _itemService,
                        _catalogService,
                        _categoryService,
                        updatePropertiesActionContext);
                    break;
            }

            return result ?? throw new ArgumentException($"Unsupported action type: {context.GetType().Name}");
        }
    }
}