namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using System.Collections.Generic;

    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkUpdateAction
    {
        BulkUpdateActionContext Context { get; }

        BulkUpdateActionResult Execute(IEnumerable<IEntity> entities);

        IBulkUpdateActionData GetActionData();

        BulkUpdateActionResult Validate();
    }
}