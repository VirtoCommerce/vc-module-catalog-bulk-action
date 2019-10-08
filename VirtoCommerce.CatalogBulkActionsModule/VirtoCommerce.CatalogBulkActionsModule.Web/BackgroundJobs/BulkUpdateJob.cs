namespace VirtoCommerce.CatalogBulkActionsModule.Web.BackgroundJobs
{
    using System;

    using Hangfire;
    using Hangfire.Server;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Extensions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.Platform.Core.PushNotifications;
    using VirtoCommerce.Platform.Data.Common;

    public class BulkUpdateJob
    {
        private readonly IBulkUpdateActionExecutor _bulkUpdateActionExecutor;

        private readonly IPushNotificationManager _pushNotificationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdateJob"/> class.
        /// </summary>
        /// <param name="bulkUpdateActionRegistrar">
        /// The bulk update action registrar.
        /// </param>
        /// <param name="pushNotificationManager">
        /// The push notification manager.
        /// </param>
        /// <param name="bulkUpdateActionExecutor">
        /// The bulk update action executor.
        /// </param>
        public BulkUpdateJob(
            IBulkUpdateActionRegistrar bulkUpdateActionRegistrar,
            IPushNotificationManager pushNotificationManager,
            IBulkUpdateActionExecutor bulkUpdateActionExecutor)
        {
            _pushNotificationManager = pushNotificationManager;
            _bulkUpdateActionExecutor = bulkUpdateActionExecutor;
        }

        public void Execute(
            BulkUpdateActionContext bulkUpdateActionContext,
            BulkUpdatePushNotification notification,
            IJobCancellationToken cancellationToken,
            PerformContext performContext)
        {
            if (bulkUpdateActionContext == null)
            {
                throw new ArgumentNullException(nameof(bulkUpdateActionContext));
            }

            if (performContext == null)
            {
                throw new ArgumentNullException(nameof(performContext));
            }

            void progressCallback(BulkUpdateProgressContext x)
            {
                notification.Patch(x);
                notification.JobId = performContext.BackgroundJob.Id;
                _pushNotificationManager.Upsert(notification);
            }

            try
            {
                _bulkUpdateActionExecutor.Execute(
                    bulkUpdateActionContext,
                    progressCallback,
                    new JobCancellationTokenWrapper(cancellationToken));
            }
            catch (JobAbortedException)
            {
                // idle
            }
            catch (Exception ex)
            {
                notification.Errors.Add(ex.ExpandExceptionMessage());
            }
            finally
            {
                notification.Description = "Update finished";
                notification.Finished = DateTime.UtcNow;
                _pushNotificationManager.Upsert(notification);
            }
        }
    }
}