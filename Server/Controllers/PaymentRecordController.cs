using GIBS.Module.DesignRequest.Models;
using GIBS.Module.DesignRequest.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Security;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net;

namespace GIBS.Module.DesignRequest.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class PaymentRecordController : ModuleControllerBase
    {
        private readonly IDesignRequestRepository _repository;

        public PaymentRecordController(IDesignRequestRepository repository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<PaymentRecord> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _repository.GetPaymentRecords(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access PaymentRecords for Module {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("user/{userid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<PaymentRecord> GetByUser(int userid, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                // Security check: Either user viewing own data, or Admin/Host viewing user data
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Allow if Admin, Host, or if the ID matches the current user
                if (User.IsInRole(RoleNames.Admin) ||
                    User.IsInRole(RoleNames.Host) ||
                    (currentUserId != null && currentUserId == userid.ToString()))
                {
                    return _repository.GetPaymentRecordsByUser(ModuleId, userid);
                }

                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access PaymentRecords for Module {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public PaymentRecord Get(int id)
        {
            PaymentRecord payment = _repository.GetPaymentRecord(id);
            if (payment != null)
            {
                // Optional: Check if user owns this payment or is admin, currently basic ViewModule check
                // For stricter security, we could verify ownership here similar to GetByUser
                return payment;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access PaymentRecord {PaymentId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public PaymentRecord Post([FromBody] PaymentRecord paymentRecord)
        {
            if (ModelState.IsValid)
            {
                paymentRecord = _repository.AddPaymentRecord(paymentRecord);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "PaymentRecord Added {PaymentRecord}", paymentRecord);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Error Adding PaymentRecord {PaymentRecord}", paymentRecord);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                paymentRecord = null;
            }
            return paymentRecord;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public PaymentRecord Put(int id, [FromBody] PaymentRecord paymentRecord)
        {
            if (ModelState.IsValid && _repository.GetPaymentRecord(id) != null)
            {
                paymentRecord = _repository.UpdatePaymentRecord(paymentRecord);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "PaymentRecord Updated {PaymentRecord}", paymentRecord);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Error Updating PaymentRecord {PaymentRecord}", paymentRecord);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                paymentRecord = null;
            }
            return paymentRecord;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            PaymentRecord payment = _repository.GetPaymentRecord(id);
            if (payment != null)
            {
                _repository.DeletePaymentRecord(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "PaymentRecord Deleted {PaymentId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Error Deleting PaymentRecord {PaymentId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}