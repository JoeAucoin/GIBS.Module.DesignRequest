using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using GIBS.Module.DesignRequest.Repository;
using Oqtane.Controllers;
using System.Net;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class UserCreditController : ModuleControllerBase
    {
        private readonly IDesignRequestRepository _repository;

        public UserCreditController(IDesignRequestRepository repository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<UserCredit> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _repository.GetUserCredits(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access UserCredits for Module {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public UserCredit Get(int id)
        {
            UserCredit userCredit = _repository.GetUserCredit(id);
            if (userCredit != null && IsAuthorizedEntityId(EntityNames.Module, userCredit.ModuleId))
            {
                return userCredit;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access UserCredit {UserCreditId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("user/{userid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public UserCredit GetByUser(int userid, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _repository.GetUserCreditByUser(ModuleId, userid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access UserCredit for Module {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public UserCredit Post([FromBody] UserCredit userCredit)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, userCredit.ModuleId))
            {
                userCredit = _repository.AddUserCredit(userCredit);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "UserCredit Added {UserCredit}", userCredit);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "User Not Authorized To Add UserCredit {UserCredit}", userCredit);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                userCredit = null;
            }
            return userCredit;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public UserCredit Put(int id, [FromBody] UserCredit userCredit)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, userCredit.ModuleId) && _repository.GetUserCredit(id) != null)
            {
                userCredit = _repository.UpdateUserCredit(userCredit);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "UserCredit Updated {UserCredit}", userCredit);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "User Not Authorized To Update UserCredit {UserCredit}", userCredit);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                userCredit = null;
            }
            return userCredit;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            UserCredit userCredit = _repository.GetUserCredit(id);
            if (userCredit != null && IsAuthorizedEntityId(EntityNames.Module, userCredit.ModuleId))
            {
                _repository.DeleteUserCredit(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "UserCredit Deleted {UserCreditId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "User Not Authorized To Delete UserCredit {UserCreditId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}