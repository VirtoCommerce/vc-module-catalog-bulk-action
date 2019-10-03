namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.Platform.Core.Common;

    public interface IBulkUpdateActionExecutor
    {
        void Execute(BulkUpdateActionContext context, Action<BulkUpdateProgressInfo> progressCallback, ICancellationToken token);
    }
}