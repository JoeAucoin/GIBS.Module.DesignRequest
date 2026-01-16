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
    public class CreditPackageController : ModuleControllerBase
    {
        private readonly IDesignRequestRepository _repository;

        public CreditPackageController(IDesignRequestRepository repository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<CreditPackage> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _repository.GetCreditPackages(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access CreditPackages for Module {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public CreditPackage Get(int id)
        {
            CreditPackage creditPackage = _repository.GetCreditPackage(id);
            if (creditPackage != null && IsAuthorizedEntityId(EntityNames.Module, creditPackage.ModuleId))
            {
                return creditPackage;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access CreditPackage {CreditPackageId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public CreditPackage Post([FromBody] CreditPackage creditPackage)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, creditPackage.ModuleId))
            {
                creditPackage = _repository.AddCreditPackage(creditPackage);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "CreditPackage Added {CreditPackage}", creditPackage);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "User Not Authorized To Add CreditPackage {CreditPackage}", creditPackage);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                creditPackage = null;
            }
            return creditPackage;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public CreditPackage Put(int id, [FromBody] CreditPackage creditPackage)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, creditPackage.ModuleId) && _repository.GetCreditPackage(id) != null)
            {
                creditPackage = _repository.UpdateCreditPackage(creditPackage);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "CreditPackage Updated {CreditPackage}", creditPackage);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "User Not Authorized To Update CreditPackage {CreditPackage}", creditPackage);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                creditPackage = null;
            }
            return creditPackage;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            CreditPackage creditPackage = _repository.GetCreditPackage(id);
            if (creditPackage != null && IsAuthorizedEntityId(EntityNames.Module, creditPackage.ModuleId))
            {
                _repository.DeleteCreditPackage(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "CreditPackage Deleted {CreditPackageId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "User Not Authorized To Delete CreditPackage {CreditPackageId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}