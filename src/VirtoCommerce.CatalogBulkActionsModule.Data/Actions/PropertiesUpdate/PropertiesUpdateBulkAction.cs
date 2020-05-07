namespace VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.BulkActionsModule.Core.Models.BulkActions;
    using VirtoCommerce.CatalogBulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.CatalogModule.Web.Converters;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;
    using VirtoCommerce.Platform.Core.Common;

    using CatalogModule = VirtoCommerce.CatalogModule.Web.Model;

    public class PropertiesUpdateBulkAction : IBulkAction
    {
        private readonly PropertiesUpdateBulkActionContext _context;

        private readonly ILazyServiceProvider _lazyServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesUpdateBulkAction"/> class.
        /// </summary>
        /// <param name="lazyServiceProvider">
        /// The lazy service provider.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public PropertiesUpdateBulkAction(ILazyServiceProvider lazyServiceProvider, PropertiesUpdateBulkActionContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _lazyServiceProvider = lazyServiceProvider;
        }

        public BulkActionContext Context => _context;

        public BulkActionResult Execute(IEnumerable<IEntity> entities)
        {
            var itemService = _lazyServiceProvider.Resolve<IItemService>();
            var bulkPropertyUpdateManager = _lazyServiceProvider.Resolve<IBulkPropertyUpdateManager>();
            var entries = entities.Cast<CatalogModule.ListEntry>().ToArray();

            if (entries.Any(entry => !entry.Type.EqualsInvariant(CatalogModule.ListEntryProduct.TypeName)))
            {
                throw new ArgumentException($"{GetType().Name} could be applied to product entities only.");
            }

            var productQuery = entries.Where(entry => entry.Type.EqualsInvariant(CatalogModule.ListEntryProduct.TypeName));
            var productIds = productQuery.Select(entry => entry.Id).ToArray();
            var products = itemService.GetByIds(
                productIds,
                ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties);

            return bulkPropertyUpdateManager.UpdateProperties(products, _context.Properties);
        }

        public object GetActionData()
        {
            var bulkPropertyUpdateManager = _lazyServiceProvider.Resolve<IBulkPropertyUpdateManager>();

            var properties = bulkPropertyUpdateManager.GetProperties(_context);

            return new { Properties = properties.Select(CreateModel).ToArray() };
        }

        public BulkActionResult Validate()
        {
            return BulkActionResult.Success;
        }

        private CatalogModule.Property CreateModel(Property property)
        {
            // note: in the called code we try to get access to validation rules and if they are have a null reference we will get an error
            property.ValidationRules = new List<PropertyValidationRule>();

            return property.ToWebModel();
        }
    }
}