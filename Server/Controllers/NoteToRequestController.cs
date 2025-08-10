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
    public class NoteToRequestController : ModuleControllerBase
    {
        private readonly IDesignRequestService _designRequestService;

        public NoteToRequestController(IDesignRequestService designRequestService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _designRequestService = designRequestService;
        }

        // GET: api/<controller>?designrequestid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<NoteToRequest>>> Get(string designrequestid)
        {
            return Ok(await _designRequestService.GetNoteToRequestsAsync(int.Parse(designrequestid), _entityId));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<NoteToRequest>> Get(int id)
        {
            var noteToRequest = await _designRequestService.GetNoteToRequestAsync(id, _entityId);
            if (noteToRequest == null)
            {
                return NotFound();
            }
            return Ok(noteToRequest);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<NoteToRequest>> Post([FromBody] NoteToRequest noteToRequest)
        {
            if (ModelState.IsValid)
            {
                noteToRequest = await _designRequestService.AddNoteToRequestAsync(noteToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "NoteToRequest Added {NoteToRequest}", noteToRequest);
            }
            return noteToRequest;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<NoteToRequest>> Put(int id, [FromBody] NoteToRequest noteToRequest)
        {
            if (ModelState.IsValid && noteToRequest.NoteId == id)
            {
                noteToRequest = await _designRequestService.UpdateNoteToRequestAsync(noteToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "NoteToRequest Updated {NoteToRequest}", noteToRequest);
            }
            return noteToRequest;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<IActionResult> Delete(int id)
        {
            await _designRequestService.DeleteNoteToRequestAsync(id, _entityId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "NoteToRequest Deleted {NoteId}", id);
            return NoContent();
        }
    }
}