namespace VirtoCommerce.CatalogBulkActionsModule.Data.Extensions
{
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;

    public static class BulkUpdatePushNotificationExtensions
    {
        public static void Patch(this BulkUpdatePushNotification target, BulkUpdateProgressContext source)
        {
            target.Description = source.Description;
            target.Errors = source.Errors;
            target.ProcessedCount = source.ProcessedCount;
            target.TotalCount = source.TotalCount;
        }
    }
}