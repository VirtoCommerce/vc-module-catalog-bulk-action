namespace VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionAbstractions
{
    using System.Collections.Generic;

    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;
    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkAction
    {
        BulkActionContext Context { get; }

        BulkActionResult Execute(IEnumerable<IEntity> entities);

        object GetActionData();

        BulkActionResult Validate();
    }
}