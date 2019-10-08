namespace VirtoCommerce.CatalogBulkActionsModule.Core.Services
{
    using System.Collections.Generic;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Platform.Core.Common;

    public interface IListEntryMover<T>
        where T : class, IEntity
    {
        void ConfirmMove(IEnumerable<T> entities);

        List<T> PrepareMove(MoveOperationContext moveOperationContext);
    }
}