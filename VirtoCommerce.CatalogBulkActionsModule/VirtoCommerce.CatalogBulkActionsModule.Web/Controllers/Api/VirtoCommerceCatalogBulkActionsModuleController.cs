namespace VirtoCommerce.CatalogBulkActionsModule.Web.Controllers.Api
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Hangfire;

    using VirtoCommerce.CatalogBulkActionsModule.Data.Models.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services.Abstractions;
    using VirtoCommerce.CatalogBulkActionsModule.Web.BackgroundJobs;
    using VirtoCommerce.CatalogBulkActionsModule.Web.Models;
    using VirtoCommerce.Platform.Core.Security;
    using VirtoCommerce.Platform.Core.Web.Security;

    [RoutePrefix("api/bulkUpdate")]
    public class VirtoCommerceCatalogBulkActionsModuleController : ApiController
    {
        private readonly IBulkActionRegistrar bulkActionRegistrar;

        private readonly IUserNameResolver _userNameResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceCatalogBulkActionsModuleController"/> class.
        /// </summary>
        /// <param name="bulkActionRegistrar">
        /// The bulk update action registrar.
        /// </param>
        /// <param name="userNameResolver">
        /// The user name resolver.
        /// </param>
        public VirtoCommerceCatalogBulkActionsModuleController(
            IBulkActionRegistrar bulkActionRegistrar,
            IUserNameResolver userNameResolver)
        {
            this.bulkActionRegistrar = bulkActionRegistrar;
            _userNameResolver = userNameResolver;
        }

        /// <summary>
        /// Attempts to cancel running task
        /// </summary>
        /// <param name="cancellationRequest">Cancellation request with task id</param>
        /// <returns>201 - on success</returns>
        [HttpPost]
        [Route("task/cancel")]
        [CheckPermission(Permission = BulkActionPredefinedPermissions.Execute)]
        public IHttpActionResult Cancel([FromBody] ActionCancellationRequest cancellationRequest)
        {
            BackgroundJob.Delete(cancellationRequest.JobId);
            return Ok();
        }

        /// <summary>
        /// Gets action initialization data (could be used to initialize UI).
        /// </summary>
        /// <param name="context">Context for which we want initialization data.</param>
        /// <returns>Initialization data for the given context.</returns>
        [HttpPost]
        [Route("action/data")]
        [CheckPermission(Permission = BulkActionPredefinedPermissions.Read)]
        public IHttpActionResult GetActionData([FromBody] BulkActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var actionDefinition = GetActionDefinition(context);

            if (Authorize(actionDefinition, context))
            {
                var factory = actionDefinition.Factory;
                var action = factory.Create(context);
                var actionData = action.GetActionData();
                return Ok(actionData);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Gets the list of all registered actions
        /// </summary>
        /// <returns>The list of registered actions</returns>
        [HttpGet]
        [Route("actions")]
        [ResponseType(typeof(BulkActionDefinition[]))]
        [CheckPermission(Permission = BulkActionPredefinedPermissions.Read)]
        public IHttpActionResult GetRegisteredActions()
        {
            var all = bulkActionRegistrar.GetAll();
            var array = all.ToArray();
            return Ok(array);
        }

        /// <summary>
        /// Starts bulk update background job.
        /// </summary>
        /// <param name="context">Execution context.</param>
        /// <returns>Notification with job id.</returns>
        [HttpPost]
        [Route("run")]
        [CheckPermission(Permission = BulkActionPredefinedPermissions.Execute)]
        [ResponseType(typeof(BulkActionPushNotification))]
        public IHttpActionResult Run([FromBody] BulkActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var actionDefinition = GetActionDefinition(context);

            if (Authorize(actionDefinition, context))
            {
                var notification = new BulkActionPushNotification(_userNameResolver.GetCurrentUserName())
                                       {
                                           Title = $"{context.ActionName}", Description = "Starting…"
                                       };

                var jobId = BackgroundJob.Enqueue<BulkActionJob>(
                    job => job.Execute(context, notification, JobCancellationToken.Null, null));
                notification.JobId = jobId;

                return Ok(notification);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Performs all definition security handlers checks, and returns true if all are succeeded.
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="context"></param>
        /// <returns>True if all checks are succeeded, otherwise false.</returns>
        private bool Authorize(BulkActionDefinition definition, BulkActionContext context)
        {
            // TechDebt: Need to add permission and custom authorization for bulk update.
            // For that we could use IExportSecurityHandler and IPermissionExportSecurityHandlerFactory - just need to move them to platform and remove export specific objects
            return true;
        }

        private BulkActionDefinition GetActionDefinition(BulkActionContext context)
        {
            var actionName = context.ActionName;
            var entityName = nameof(IBulkActionRegistrar);
            var message = $"Action \"{actionName}\" is not registered using \"{entityName}\".";
            return bulkActionRegistrar.GetByName(actionName) ?? throw new ArgumentException(message);
        }
    }
}