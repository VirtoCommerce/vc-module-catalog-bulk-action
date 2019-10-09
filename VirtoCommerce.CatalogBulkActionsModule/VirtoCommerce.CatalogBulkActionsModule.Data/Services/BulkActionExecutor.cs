namespace VirtoCommerce.CatalogBulkActionsModule.Data.Services
{
    using System;
    using System.Linq;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services.Abstractions;
    using VirtoCommerce.Platform.Core.Common;

    public class BulkActionExecutor : IBulkActionExecutor
    {
        private readonly IBulkActionRegistrar bulkActionRegistrar;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkActionExecutor"/> class.
        /// </summary>
        /// <param name="bulkActionRegistrar">
        /// The bulk update action registrar.
        /// </param>
        public BulkActionExecutor(IBulkActionRegistrar bulkActionRegistrar)
        {
            this.bulkActionRegistrar = bulkActionRegistrar;
        }

        public virtual void Execute(
            BulkActionContext context,
            Action<BulkActionProgressContext> progressCallback,
            ICancellationToken token)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            token.ThrowIfCancellationRequested();

            var totalCount = 0;
            var processedCount = 0;

            var progressInfo = new BulkActionProgressContext { Description = "Validation has started…", };
            progressCallback(progressInfo);

            try
            {
                var actionDefinition = bulkActionRegistrar.GetByName(context.ActionName);
                var action = actionDefinition.Factory.Create(context);

                var validationResult = action.Validate();
                var proceed = validationResult.Succeeded;

                token.ThrowIfCancellationRequested();

                if (proceed)
                {
                    progressInfo.Description = "Validation completed successfully.";
                }
                else
                {
                    progressInfo.Description = "Validation completed with errors.";
                    progressInfo.Errors = validationResult.Errors;
                }

                progressCallback(progressInfo);

                if (proceed)
                {
                    var dataSourceFactory = actionDefinition.DataSourceFactory
                                            ?? throw new ArgumentException(
                                                nameof(BulkActionDefinition.DataSourceFactory));
                    var dataSource = dataSourceFactory.Create(context);
                    totalCount = dataSource.GetTotalCount();
                    processedCount = 0;

                    progressInfo.ProcessedCount = processedCount;
                    progressInfo.TotalCount = totalCount;
                    progressInfo.Description = "Update has started…";
                    progressCallback(progressInfo);

                    while (dataSource.Fetch())
                    {
                        token.ThrowIfCancellationRequested();

                        var result = action.Execute(dataSource.Items);

                        if (result.Succeeded)
                        {
                            // idle
                        }
                        else
                        {
                            progressInfo.Errors.AddRange(result.Errors);
                        }

                        processedCount += dataSource.Items.Count();
                        progressInfo.ProcessedCount = processedCount;

                        if (processedCount == totalCount)
                        {
                            continue;
                        }

                        progressInfo.Description = $"{processedCount} out of {totalCount} have been updated.";
                        progressCallback(progressInfo);
                    }
                }
                else
                {
                    // idle
                }
            }
            catch (Exception e)
            {
                progressInfo.Errors.Add(e.Message);
            }
            finally
            {
                var message = progressInfo.Errors?.Count > 0 ? "Update completed with errors" : "Update completed";
                progressInfo.Description = $"{message}: {processedCount} out of {totalCount} have been updated.";
                progressCallback(progressInfo);
            }
        }
    }
}