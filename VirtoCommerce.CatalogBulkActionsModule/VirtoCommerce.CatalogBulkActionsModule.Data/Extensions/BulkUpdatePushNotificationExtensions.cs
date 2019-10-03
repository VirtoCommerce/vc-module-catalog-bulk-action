namespace VirtoCommerce.CatalogBulkActionsModule.Core.Extensions
{
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public static class BulkUpdatePushNotificationExtensions
    {
        public static void Patch(this BulkUpdatePushNotification target, BulkUpdateProgressInfo source)
        {
            target.Description = source.Description;
            target.Errors = source.Errors;
            target.ProcessedCount = source.ProcessedCount;
            target.TotalCount = source.TotalCount;
        }
    }
}