namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using System.Collections.Generic;

    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkUpdateAction
    {
        BulkUpdateActionContext Context { get; }
        IBulkUpdateActionData GetActionData();
        BulkUpdateActionResult Validate();
        BulkUpdateActionResult Execute(IEnumerable<IEntity> entities);
    }
}