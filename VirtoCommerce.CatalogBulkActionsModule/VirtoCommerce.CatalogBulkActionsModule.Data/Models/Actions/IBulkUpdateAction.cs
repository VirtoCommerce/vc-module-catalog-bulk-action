namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions
{
    using System.Collections.Generic;

    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkUpdateAction
    {
        BulkUpdateActionResult Execute(IEnumerable<IEntity> entities);

        IBulkUpdateActionData GetActionData();

        BulkUpdateActionResult Validate();
    }
}