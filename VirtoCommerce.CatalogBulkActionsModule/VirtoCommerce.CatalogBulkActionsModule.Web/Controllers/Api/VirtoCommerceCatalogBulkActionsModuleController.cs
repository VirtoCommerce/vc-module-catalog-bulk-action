using System.Web.Http;
using VirtoCommerce.CatalogBulkActionsModule.Core;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.CatalogBulkActionsModule.Web.Controllers.Api
{
    [RoutePrefix("api/VirtoCommerceCatalogBulkActionsModule")]
    public class VirtoCommerceCatalogBulkActionsModuleController : ApiController
    {
        // GET: api/VirtoCommerceCatalogBulkActionsModule
        [HttpGet]
        [Route("")]
        [CheckPermission(Permission = ModuleConstants.Security.Permissions.Read)]
        public IHttpActionResult Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
