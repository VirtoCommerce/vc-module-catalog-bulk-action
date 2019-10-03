namespace VirtoCommerce.CatalogBulkActionsModule.Web.BackgroundJobs
{
    using Hangfire;

    using VirtoCommerce.Platform.Core.Common;

    public class JobCancellationTokenWrapper : ICancellationToken
    {
        public IJobCancellationToken JobCancellationToken { get; }

        public JobCancellationTokenWrapper(IJobCancellationToken jobCancellationToken)
        {
            JobCancellationToken = jobCancellationToken;
        }

        #region Implementation of ICancellationToken

        public void ThrowIfCancellationRequested()
        {
            JobCancellationToken.ThrowIfCancellationRequested();
        }

        #endregion
    }
}