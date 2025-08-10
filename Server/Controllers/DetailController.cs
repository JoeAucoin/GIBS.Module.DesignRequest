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
    public class DetailController : ModuleControllerBase
    {
        private readonly IDesignRequestService _designRequestService;

        public DetailController(IDesignRequestService designRequestService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _designRequestService = designRequestService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<Detail>>> Get(string moduleid)
        {
            return Ok(await _designRequestService.GetDetailsAsync(int.Parse(moduleid)));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<Detail>> Get(int id)
        {
            var detail = await _designRequestService.GetDetailAsync(id, _entityId);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Detail>> Post([FromBody] Detail detail)
        {
            if (ModelState.IsValid)
            {
                detail = await _designRequestService.AddDetailAsync(detail);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Detail Added {Detail}", detail);
            }
            return detail;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Detail>> Put(int id, [FromBody] Detail detail)
        {
            if (ModelState.IsValid && detail.DetailId == id)
            {
                detail = await _designRequestService.UpdateDetailAsync(detail);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Detail Updated {Detail}", detail);
            }
            return detail;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<IActionResult> Delete(int id)
        {
            await _designRequestService.DeleteDetailAsync(id, _entityId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Detail Deleted {DetailId}", id);
            return NoContent();
        }
    }
}