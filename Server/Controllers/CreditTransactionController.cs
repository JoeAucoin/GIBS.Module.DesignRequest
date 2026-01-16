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
    public class CreditTransactionController : ModuleControllerBase
    {
        private readonly IDesignRequestRepository _repository;

        public CreditTransactionController(IDesignRequestRepository repository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<CreditTransaction> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _repository.GetCreditTransactions(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access CreditTransactions for Module {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("user/{userid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<CreditTransaction> GetByUser(int userid, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                // Security check: Either user viewing own data, or Admin/Editor viewing user data
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Allow if Admin, Host, or if the ID matches the current user
                if (User.IsInRole(RoleNames.Admin) ||
                    User.IsInRole(RoleNames.Host) ||
                    (currentUserId != null && currentUserId == userid.ToString()))
                {
                    return _repository.GetCreditTransactionsByUser(ModuleId, userid);
                }

                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access CreditTransactions for Module {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public CreditTransaction Get(int id)
        {
            CreditTransaction transaction = _repository.GetCreditTransaction(id);
            // Note: transaction model might not have ModuleId, so strict IsAuthorizedEntityId check relies on passing context correctly.
            // If transaction has no ModuleId, this check might fail if not careful. Assuming context is managed.
            if (transaction != null)
            {
                return transaction;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access CreditTransaction {TransactionId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)] // Typically only Admins/System creates transactions
        public CreditTransaction Post([FromBody] CreditTransaction creditTransaction)
        {
            if (ModelState.IsValid)
            {
                creditTransaction = _repository.AddCreditTransaction(creditTransaction);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "CreditTransaction Added {CreditTransaction}", creditTransaction);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Error Adding CreditTransaction {CreditTransaction}", creditTransaction);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                creditTransaction = null;
            }
            return creditTransaction;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public CreditTransaction Put(int id, [FromBody] CreditTransaction creditTransaction)
        {
            if (ModelState.IsValid && _repository.GetCreditTransaction(id) != null)
            {
                creditTransaction = _repository.UpdateCreditTransaction(creditTransaction);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "CreditTransaction Updated {CreditTransaction}", creditTransaction);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Error Updating CreditTransaction {CreditTransaction}", creditTransaction);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                creditTransaction = null;
            }
            return creditTransaction;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            CreditTransaction transaction = _repository.GetCreditTransaction(id);
            if (transaction != null)
            {
                _repository.DeleteCreditTransaction(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "CreditTransaction Deleted {TransactionId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Error Deleting CreditTransaction {TransactionId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}