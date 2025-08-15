using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using GIBS.Module.DesignRequest.Models;
using GIBS.Module.DesignRequest.Services;
using System.Threading.Tasks;
using Oqtane.Controllers;
using Microsoft.AspNetCore.Http;

namespace GIBS.Module.DesignRequest.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class ApplianceToRequestController : ModuleControllerBase
    {
        private readonly IDesignRequestService _designRequestService;

        public ApplianceToRequestController(IDesignRequestService designRequestService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _designRequestService = designRequestService;
        }

        // GET: api/<controller>?designrequestid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<ApplianceToRequest>>> Get(string designrequestid)
        {
            return Ok(await _designRequestService.GetApplianceToRequestsAsync(int.Parse(designrequestid), _entityId));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<ApplianceToRequest>> Get(int id)
        {
            var applianceToRequest = await _designRequestService.GetApplianceToRequestAsync(id, _entityId);
            if (applianceToRequest == null)
            {
                return NotFound();
            }
            return Ok(applianceToRequest);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<ApplianceToRequest>> Post([FromBody] ApplianceToRequest applianceToRequest)
        {
            if (ModelState.IsValid)
            {
                applianceToRequest = await _designRequestService.AddApplianceToRequestAsync(applianceToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "ApplianceToRequest Added {ApplianceToRequest}", applianceToRequest);
            }
            return applianceToRequest;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<ApplianceToRequest>> Put(int id, [FromBody] ApplianceToRequest applianceToRequest)
        {
            if (ModelState.IsValid && applianceToRequest.ApplianceToRequestId == id)
            {
                applianceToRequest = await _designRequestService.UpdateApplianceToRequestAsync(applianceToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "ApplianceToRequest Updated {ApplianceToRequest}", applianceToRequest);
            }
            return applianceToRequest;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<IActionResult> Delete(int id)
        {
            await _designRequestService.DeleteApplianceToRequestAsync(id, _entityId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "ApplianceToRequest Deleted {ApplianceToRequestId}", id);
            return NoContent();
        }
    }
}