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
    public class DetailToRequestController : ModuleControllerBase
    {
        private readonly IDesignRequestService _designRequestService;

        public DetailToRequestController(IDesignRequestService designRequestService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _designRequestService = designRequestService;
        }

        // GET: api/<controller>?designrequestid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<DetailToRequest>>> Get(string designrequestid)
        {
            return Ok(await _designRequestService.GetDetailToRequestsAsync(int.Parse(designrequestid), _entityId));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<DetailToRequest>> Get(int id)
        {
            var detailToRequest = await _designRequestService.GetDetailToRequestAsync(id, _entityId);
            if (detailToRequest == null)
            {
                return NotFound();
            }
            return Ok(detailToRequest);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<DetailToRequest>> Post([FromBody] DetailToRequest detailToRequest)
        {
            if (ModelState.IsValid)
            {
                detailToRequest = await _designRequestService.AddDetailToRequestAsync(detailToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "DetailToRequest Added {DetailToRequest}", detailToRequest);
            }
            return detailToRequest;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<DetailToRequest>> Put(int id, [FromBody] DetailToRequest detailToRequest)
        {
            if (ModelState.IsValid && detailToRequest.DetailToRequestId == id)
            {
                detailToRequest = await _designRequestService.UpdateDetailToRequestAsync(detailToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "DetailToRequest Updated {DetailToRequest}", detailToRequest);
            }
            return detailToRequest;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<IActionResult> Delete(int id)
        {
            await _designRequestService.DeleteDetailToRequestAsync(id, _entityId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "DetailToRequest Deleted {DetailToRequestId}", id);
            return NoContent();
        }
    }
}