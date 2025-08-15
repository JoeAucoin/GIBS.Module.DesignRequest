using GIBS.Module.DesignRequest.Models;
using GIBS.Module.DesignRequest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GIBS.Module.DesignRequest.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class DesignRequestController : ModuleControllerBase
    {
        private readonly IDesignRequestService _DesignRequestService;

        public DesignRequestController(IDesignRequestService DesignRequestService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _DesignRequestService = DesignRequestService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.DesignRequest>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _DesignRequestService.GetDesignRequestsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.DesignRequest> Get(int id, int moduleid)
        {
            Models.DesignRequest DesignRequest = await _DesignRequestService.GetDesignRequestAsync(id, moduleid);
            if (DesignRequest != null && IsAuthorizedEntityId(EntityNames.Module, DesignRequest.ModuleId))
            {
                return DesignRequest;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Get Attempt {DesignRequestId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // New endpoint for pagination
        [HttpGet("paged")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<Paged<Models.DesignRequest>>> Get(string moduleid, string page, string pagesize)
        {
            if (int.TryParse(moduleid, out int ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId) &&
                int.TryParse(page, out int Page) && int.TryParse(pagesize, out int PageSize))
            {
                return Ok(await _DesignRequestService.GetDesignRequestsAsync(ModuleId, Page, PageSize));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Paged DesignRequest Get Attempt {ModuleId}", moduleid);
                return Forbid();
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.DesignRequest> Post([FromBody] Models.DesignRequest DesignRequest)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, DesignRequest.ModuleId))
            {
                DesignRequest = await _DesignRequestService.AddDesignRequestAsync(DesignRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Post Attempt {DesignRequest}", DesignRequest);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                DesignRequest = null;
            }
            return DesignRequest;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.DesignRequest> Put(int id, [FromBody] Models.DesignRequest DesignRequest)
        {
            if (ModelState.IsValid && DesignRequest.DesignRequestId == id && IsAuthorizedEntityId(EntityNames.Module, DesignRequest.ModuleId))
            {
                DesignRequest = await _DesignRequestService.UpdateDesignRequestAsync(DesignRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Put Attempt {DesignRequest}", DesignRequest);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                DesignRequest = null;
            }
            return DesignRequest;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.DesignRequest DesignRequest = await _DesignRequestService.GetDesignRequestAsync(id, moduleid);
            if (DesignRequest != null && IsAuthorizedEntityId(EntityNames.Module, DesignRequest.ModuleId))
            {
                await _DesignRequestService.DeleteDesignRequestAsync(id, DesignRequest.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Delete Attempt {DesignRequestId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
