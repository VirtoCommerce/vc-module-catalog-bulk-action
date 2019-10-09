namespace VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions.Abstractions
{
    using System.Collections.Generic;

    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkAction
    {
        BulkActionContext Context { get; }

        BulkActionResult Execute(IEnumerable<IEntity> entities);

        object GetActionData();

        BulkActionResult Validate();
    }
}