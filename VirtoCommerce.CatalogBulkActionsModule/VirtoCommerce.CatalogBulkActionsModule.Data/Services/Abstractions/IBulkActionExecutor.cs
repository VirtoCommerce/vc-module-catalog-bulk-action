namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services.Abstractions
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkActionExecutor
    {
        void Execute(
            BulkActionContext context,
            Action<BulkActionProgressContext> progressCallback,
            ICancellationToken token);
    }
}