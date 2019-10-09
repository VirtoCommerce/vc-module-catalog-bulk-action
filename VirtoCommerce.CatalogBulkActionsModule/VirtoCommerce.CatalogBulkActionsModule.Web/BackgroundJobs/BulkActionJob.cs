namespace VirtoCommerce.CatalogBulkActionsModule.Web.BackgroundJobs
{
    using System;

    using Hangfire;
    using Hangfire.Server;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Extensions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services.Abstractions;
    using VirtoCommerce.Platform.Core.PushNotifications;
    using VirtoCommerce.Platform.Data.Common;

    public class BulkActionJob
    {
        private readonly IBulkActionExecutor bulkActionExecutor;

        private readonly IPushNotificationManager _pushNotificationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkActionJob"/> class.
        /// </summary>
        /// <param name="bulkActionRegistrar">
        /// The bulk update action registrar.
        /// </param>
        /// <param name="pushNotificationManager">
        /// The push notification manager.
        /// </param>
        /// <param name="bulkActionExecutor">
        /// The bulk update action executor.
        /// </param>
        public BulkActionJob(
            IBulkActionRegistrar bulkActionRegistrar,
            IPushNotificationManager pushNotificationManager,
            IBulkActionExecutor bulkActionExecutor)
        {
            _pushNotificationManager = pushNotificationManager;
            this.bulkActionExecutor = bulkActionExecutor;
        }

        public void Execute(
            BulkActionContext bulkActionContext,
            BulkActionPushNotification notification,
            IJobCancellationToken cancellationToken,
            PerformContext performContext)
        {
            if (bulkActionContext == null)
            {
                throw new ArgumentNullException(nameof(bulkActionContext));
            }

            if (performContext == null)
            {
                throw new ArgumentNullException(nameof(performContext));
            }

            void progressCallback(BulkActionProgressContext x)
            {
                notification.Patch(x);
                notification.JobId = performContext.BackgroundJob.Id;
                _pushNotificationManager.Upsert(notification);
            }

            try
            {
                bulkActionExecutor.Execute(
                    bulkActionContext,
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
                notification.Description = "Job finished";
                notification.Finished = DateTime.UtcNow;
                _pushNotificationManager.Upsert(notification);
            }
        }
    }
}