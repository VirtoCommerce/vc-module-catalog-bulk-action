namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;

    public class BulkActionFactory : IBulkActionFactory
    {
        private readonly ILazyServiceProvider _lazyLazyServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkActionFactory"/> class.
        /// </summary>
        /// <param name="lazyServiceProvider">
        /// The service provider.
        /// </param>
        public BulkActionFactory(ILazyServiceProvider lazyServiceProvider)
        {
            _lazyLazyServiceProvider = lazyServiceProvider;
        }

        public IBulkAction Create(BulkActionContext context)
        {
            IBulkAction result = null;

            switch (context)
            {
                case CategoryChangeBulkActionContext changeCategoryActionContext:
                    result = new CategoryChangeBulkAction(_lazyLazyServiceProvider, changeCategoryActionContext);
                    break;

                case PropertiesUpdateBulkActionContext updatePropertiesActionContext:
                    result = new PropertiesUpdateBulkAction(_lazyLazyServiceProvider, updatePropertiesActionContext);
                    break;
            }

            return result ?? throw new ArgumentException($"Unsupported action type: {context.GetType().Name}");
        }
    }
}