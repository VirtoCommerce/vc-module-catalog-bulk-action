namespace VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionAbstractions
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Core.BulkActionModels;
    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkActionExecutor
    {
        void Execute(
            BulkActionContext context,
            Action<BulkActionProgressContext> progressCallback,
            ICancellationToken token);
    }
}