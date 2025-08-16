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
    [Route(ControllerRoutes.Default)]
    public class NotificationToRequestController : ModuleControllerBase
    {
        private readonly IDesignRequestService _designRequestService;

        public NotificationToRequestController(IDesignRequestService designRequestService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _designRequestService = designRequestService;
        }

        // GET: api/<controller>/GetByDesignRequest/5?moduleid=x
        [HttpGet("GetByDesignRequest/{designRequestId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<NotificationToRequest>> GetByDesignRequest(int designRequestId, [FromQuery] int moduleid)
        {
            return await _designRequestService.GetNotificationToRequestsAsync(designRequestId, moduleid);
        }

        // GET api/<controller>/5?moduleid=x
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<NotificationToRequest> Get(int id, [FromQuery] int moduleid)
        {
            return await _designRequestService.GetNotificationToRequestAsync(id, moduleid);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<NotificationToRequest> Post([FromBody] NotificationToRequest notificationToRequest)
        {
            if (ModelState.IsValid)
            {
                notificationToRequest = await _designRequestService.AddNotificationToRequestAsync(notificationToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Notification To Request Added {notificationToRequest}", notificationToRequest);
            }
            return notificationToRequest;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<NotificationToRequest> Put(int id, [FromBody] NotificationToRequest notificationToRequest)
        {
            if (ModelState.IsValid)
            {
                notificationToRequest = await _designRequestService.UpdateNotificationToRequestAsync(notificationToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Notification To Request Updated {notificationToRequest}", notificationToRequest);
            }
            return notificationToRequest;
        }

        // DELETE api/<controller>/5?moduleid=x
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, [FromQuery] int moduleid)
        {
            await _designRequestService.DeleteNotificationToRequestAsync(id, moduleid);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Notification To Request Deleted {NotificationToRequestId}", id);
        }
    }
}