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
    public class ApplianceController : ModuleControllerBase
    {
        private readonly IDesignRequestService _designRequestService;

        public ApplianceController(IDesignRequestService designRequestService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _designRequestService = designRequestService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<Appliance>>> Get(string moduleid)
        {
            return Ok(await _designRequestService.GetAppliancesAsync(int.Parse(moduleid)));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<Appliance>> Get(int id)
        {
            var appliance = await _designRequestService.GetApplianceAsync(id, _entityId);
            if (appliance == null)
            {
                return NotFound();
            }
            return Ok(appliance);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Appliance>> Post([FromBody] Appliance appliance)
        {
            if (ModelState.IsValid)
            {
                appliance = await _designRequestService.AddApplianceAsync(appliance);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Appliance Added {Appliance}", appliance);
            }
            return appliance;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Appliance>> Put(int id, [FromBody] Appliance appliance)
        {
            if (ModelState.IsValid && appliance.ApplianceId == id)
            {
                appliance = await _designRequestService.UpdateApplianceAsync(appliance);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Appliance Updated {Appliance}", appliance);
            }
            return appliance;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<IActionResult> Delete(int id)
        {
            await _designRequestService.DeleteApplianceAsync(id, _entityId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Appliance Deleted {ApplianceId}", id);
            return NoContent();
        }
    }
}