namespace VirtoCommerce.CatalogBulkActionsModule.Core.Services
{
    using System.Collections.Generic;

    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.Platform.Core.Common;

    public interface IMover<T>
        where T : class, IEntity
    {
        void Confirm(IEnumerable<T> entities);

        List<T> Prepare(MoveOperationContext moveOperationContext);
    }
}