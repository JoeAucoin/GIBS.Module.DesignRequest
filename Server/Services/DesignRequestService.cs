using GIBS.Module.DesignRequest.Repository;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Security;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Services
{
    public class ServerDesignRequestService : IDesignRequestService
    {
        private readonly IDesignRequestRepository _DesignRequestRepository;
        private readonly IApplianceRepository _applianceRepository;
        private readonly IDetailRepository _detailRepository;
        private readonly IApplianceToRequestRepository _applianceToRequestRepository;
        private readonly IDetailToRequestRepository _detailToRequestRepository;
        private readonly INoteToRequestRepository _noteToRequestRepository;
        private readonly IFileToRequestRepository _fileToRequestRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;
        private readonly INotificationRepository _notifications;
        private readonly IUserRepository _userRepository;

        public ServerDesignRequestService(IDesignRequestRepository DesignRequestRepository, IApplianceRepository applianceRepository, IDetailRepository detailRepository, IApplianceToRequestRepository applianceToRequestRepository, IDetailToRequestRepository detailToRequestRepository, INoteToRequestRepository noteToRequestRepository, IFileToRequestRepository fileToRequestRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor, INotificationRepository notifications, IUserRepository userRepository)
        {
            _DesignRequestRepository = DesignRequestRepository;
            _applianceRepository = applianceRepository;
            _detailRepository = detailRepository;
            _applianceToRequestRepository = applianceToRequestRepository;
            _detailToRequestRepository = detailToRequestRepository;
            _noteToRequestRepository = noteToRequestRepository;
            _fileToRequestRepository = fileToRequestRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
            _notifications = notifications;
            _userRepository = userRepository;
        }

        // Other service methods...
        public Task<List<User>> GetUsersAsync()
        {
            // use the server-side IUserRepository to get the users
            return Task.FromResult(_userRepository.GetUsers().ToList());
        }

        public Task<List<Models.DesignRequest>> GetDesignRequestsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_DesignRequestRepository.GetDesignRequests(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.DesignRequest> GetDesignRequestAsync(int DesignRequestId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_DesignRequestRepository.GetDesignRequest(DesignRequestId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Get Attempt {DesignRequestId} {ModuleId}", DesignRequestId, ModuleId);
                return null;
            }
        }

        //public async Task<Models.DesignRequest> AddDesignRequestAsync(Models.DesignRequest DesignRequest)
        //{
        //    // check honeypot field
        //    if (!string.IsNullOrEmpty(DesignRequest.Fax))
        //    {
        //        _logger.Log(LogLevel.Error, this, LogFunction.Security, "Bot submission detected based on honeypot field.");
        //        return await Task.FromResult<Models.DesignRequest>(null); // Ensure async behavior
        //    }

        //    Models.DesignRequest savedDesignRequest;

        //    // check for authenticated users with edit permissions
        //    if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, DesignRequest.ModuleId, PermissionNames.Edit))
        //    {
        //        savedDesignRequest = _DesignRequestRepository.AddDesignRequest(DesignRequest);
        //        _logger.Log(LogLevel.Information, this, LogFunction.Create, "DesignRequest Added {DesignRequest}", savedDesignRequest);
        //    }
        //    else
        //    {
        //        // handle anonymous submissions
        //        var site = _alias.SiteId;
        //        var request = _accessor.HttpContext.Request;
        //        var mySite = $"{request.Scheme}://{request.Host}{request.PathBase}";

        //        var body = new StringBuilder();
        //        body.AppendLine($"<p>You have received a new design request form submission:</p>");
        //        body.AppendLine($"<p><b>Contact Name:</b> {DesignRequest.ContactName}</p>");
        //        body.AppendLine($"<p><b>Company:</b> {DesignRequest.Company}</p>");
        //        body.AppendLine($"<p><b>Address:</b> {DesignRequest.Address}</p>");
        //        body.AppendLine($"<p><b>Phone:</b> {DesignRequest.Phone}</p>");
        //        body.AppendLine($"<p><b>Email:</b> {DesignRequest.Email}</p>");
        //        body.AppendLine($"<p><b>Website:</b> {DesignRequest.Website}</p>");
        //        body.AppendLine($"<p><b>Interest:</b> {DesignRequest.Interest}</p>");
        //        body.AppendLine($"<p><b>Questions/Comments:</b></p>");
        //        body.AppendLine($"<p>{DesignRequest.QuestionComments}</p>");
        //        body.AppendLine($"<hr />");
        //        body.AppendLine($"<p><b>Submitted On:</b> {DesignRequest.CreatedOn}</p>");
        //        body.AppendLine($"<p><b>IP Address:</b> https://ip-address-lookup-v4.com/ip/{DesignRequest.IP_Address}</p>");
        //        body.AppendLine($"<p><b>Site:</b> " + mySite.ToString() + "</p>");

        //        var sendon = DateTime.UtcNow;
        //        var sendtoname = DesignRequest.SendToName ?? "Contact Form Submission";
        //        var sendtoemail = DesignRequest.SendToEmail ?? "";

        //        // sanitize the input to prevent XSS attacks
        //        DesignRequest.ContactName = WebUtility.HtmlEncode(DesignRequest.ContactName);
        //        DesignRequest.Company = WebUtility.HtmlEncode(DesignRequest.Company);
        //        DesignRequest.Address = WebUtility.HtmlEncode(DesignRequest.Address);
        //        DesignRequest.Phone = WebUtility.HtmlEncode(DesignRequest.Phone);
        //        DesignRequest.Email = WebUtility.HtmlEncode(DesignRequest.Email);
        //        DesignRequest.Website = WebUtility.HtmlEncode(DesignRequest.Website);
        //        DesignRequest.QuestionComments = WebUtility.HtmlEncode(DesignRequest.QuestionComments);

        //        savedDesignRequest = _DesignRequestRepository.AddDesignRequest(DesignRequest);
        //        _logger.Log(LogLevel.Information, this, LogFunction.Create, "DesignRequest Added {DesignRequest}", savedDesignRequest);

        //        var recordID = savedDesignRequest.DesignRequestId;
        //        var subject = $"DesignRequest Form Submission - " + recordID.ToString();

        //        var notification = new Notification(site, sendtoname.ToString(), sendtoemail.ToString(), subject, body.ToString(), sendon);
        //        _notifications.AddNotification(notification);
        //        _logger.Log(LogLevel.Information, this, LogFunction.Create, "Notification Added", notification);
        //    }

        //    return await Task.FromResult(savedDesignRequest);
        //}

        public async Task<Models.DesignRequest> AddDesignRequestAsync(Models.DesignRequest DesignRequest)
        {
            // check honeypot field
            if (!string.IsNullOrEmpty(DesignRequest.Fax))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Bot submission detected based on honeypot field.");
                return await Task.FromResult<Models.DesignRequest>(null); // Ensure async behavior
            }

            // Sanitize the input to prevent XSS attacks before saving
            DesignRequest.ContactName = WebUtility.HtmlEncode(DesignRequest.ContactName);
            DesignRequest.Company = WebUtility.HtmlEncode(DesignRequest.Company);
            DesignRequest.Address = WebUtility.HtmlEncode(DesignRequest.Address);
            DesignRequest.Phone = WebUtility.HtmlEncode(DesignRequest.Phone);
            DesignRequest.Email = WebUtility.HtmlEncode(DesignRequest.Email);
            DesignRequest.Website = WebUtility.HtmlEncode(DesignRequest.Website);
            DesignRequest.QuestionComments = WebUtility.HtmlEncode(DesignRequest.QuestionComments);
            DesignRequest.OverallSpaceDimensions = WebUtility.HtmlEncode(DesignRequest.OverallSpaceDimensions);
            DesignRequest.CeilingHeight = WebUtility.HtmlEncode(DesignRequest.CeilingHeight);
            DesignRequest.LengthOfKitchen = WebUtility.HtmlEncode(DesignRequest.LengthOfKitchen);
            DesignRequest.SlopeOfPatio = WebUtility.HtmlEncode(DesignRequest.SlopeOfPatio);
            DesignRequest.ShapeConfiguration = WebUtility.HtmlEncode(DesignRequest.ShapeConfiguration);
            DesignRequest.DoorStyle = WebUtility.HtmlEncode(DesignRequest.DoorStyle);
            DesignRequest.Color = WebUtility.HtmlEncode(DesignRequest.Color);
            DesignRequest.CountertopThickness = WebUtility.HtmlEncode(DesignRequest.CountertopThickness);
            DesignRequest.CounterDepth = WebUtility.HtmlEncode(DesignRequest.CounterDepth);
            DesignRequest.CounterHeight = WebUtility.HtmlEncode(DesignRequest.CounterHeight);
            DesignRequest.Status = WebUtility.HtmlEncode(DesignRequest.Status);

            // Save the design request to the database
            var savedDesignRequest = _DesignRequestRepository.AddDesignRequest(DesignRequest);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "DesignRequest Added {DesignRequest}", savedDesignRequest);

            // If the user is anonymous, send a notification email
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, DesignRequest.ModuleId, PolicyNames.EditModule))
            {
                var site = _alias.SiteId;
                var request = _accessor.HttpContext.Request;
                var mySite = $"{request.Scheme}://{request.Host}{request.PathBase}";

                var body = new StringBuilder();
                body.AppendLine($"<p>You have received a new design request form submission:</p>");
                body.AppendLine($"<p><b>Contact Name:</b> {savedDesignRequest.ContactName}</p>");
                body.AppendLine($"<p><b>Company:</b> {savedDesignRequest.Company}</p>");
                body.AppendLine($"<p><b>Address:</b> {savedDesignRequest.Address}</p>");
                body.AppendLine($"<p><b>Phone:</b> {savedDesignRequest.Phone}</p>");
                body.AppendLine($"<p><b>Email:</b> {savedDesignRequest.Email}</p>");
                body.AppendLine($"<p><b>Website:</b> {savedDesignRequest.Website}</p>");
                body.AppendLine($"<p><b>Interest:</b> {savedDesignRequest.Interest}</p>");
                body.AppendLine($"<p><b>Questions/Comments:</b></p>");
                body.AppendLine($"<p>{savedDesignRequest.QuestionComments}</p>");

                if (savedDesignRequest.InstallationDate != DateTime.MinValue)
                {
                    body.AppendLine($"<p><b>Installation Date:</b> {savedDesignRequest.InstallationDate:d}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.OverallSpaceDimensions))
                {
                    body.AppendLine($"<p><b>Overall Space Dimensions:</b> {savedDesignRequest.OverallSpaceDimensions}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.CeilingHeight))
                {
                    body.AppendLine($"<p><b>Ceiling Height:</b> {savedDesignRequest.CeilingHeight}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.LengthOfKitchen))
                {
                    body.AppendLine($"<p><b>Length Of Kitchen:</b> {savedDesignRequest.LengthOfKitchen}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.SlopeOfPatio))
                {
                    body.AppendLine($"<p><b>Slope Of Patio:</b> {savedDesignRequest.SlopeOfPatio}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.ShapeConfiguration))
                {
                    body.AppendLine($"<p><b>Shape/Configuration:</b> {savedDesignRequest.ShapeConfiguration}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.DoorStyle))
                {
                    body.AppendLine($"<p><b>Door Style:</b> {savedDesignRequest.DoorStyle}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.Color))
                {
                    body.AppendLine($"<p><b>Color:</b> {savedDesignRequest.Color}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.CountertopThickness))
                {
                    body.AppendLine($"<p><b>Countertop Thickness:</b> {savedDesignRequest.CountertopThickness}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.CounterDepth))
                {
                    body.AppendLine($"<p><b>Counter Depth:</b> {savedDesignRequest.CounterDepth}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.CounterHeight))
                {
                    body.AppendLine($"<p><b>Counter Height:</b> {savedDesignRequest.CounterHeight}</p>");
                }
                if (!string.IsNullOrEmpty(savedDesignRequest.Status))
                {
                    body.AppendLine($"<p><b>Status:</b> {savedDesignRequest.Status}</p>");
                }

                body.AppendLine($"<hr />");
                body.AppendLine($"<p><b>Submitted On:</b> {savedDesignRequest.CreatedOn}</p>");
                body.AppendLine($"<p><b>IP Address:</b> https://ip-address-lookup-v4.com/ip/{savedDesignRequest.IP_Address}</p>");
                body.AppendLine($"<p><b>Site:</b> " + mySite.ToString() + "</p>");

                var sendon = DateTime.UtcNow;
                var sendtoname = savedDesignRequest.SendToName ?? "Contact Form Submission";
                var sendtoemail = savedDesignRequest.SendToEmail ?? "";
                var recordID = savedDesignRequest.DesignRequestId;
                var subject = $"DesignRequest Form Submission - " + recordID.ToString();

                var notification = new Notification(site, sendtoname, sendtoemail, subject, body.ToString(), sendon);
                _notifications.AddNotification(notification);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Notification Added", notification);
            }

            return await Task.FromResult(savedDesignRequest);
        }

        public Task<Models.DesignRequest> UpdateDesignRequestAsync(Models.DesignRequest DesignRequest)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, DesignRequest.ModuleId, PermissionNames.Edit))
            {
                DesignRequest = _DesignRequestRepository.UpdateDesignRequest(DesignRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "DesignRequest Updated {DesignRequest}", DesignRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Update Attempt {DesignRequest}", DesignRequest);
                DesignRequest = null;
            }
            return Task.FromResult(DesignRequest);
        }

        public Task DeleteDesignRequestAsync(int DesignRequestId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _DesignRequestRepository.DeleteDesignRequest(DesignRequestId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "DesignRequest Deleted {DesignRequestId}", DesignRequestId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DesignRequest Delete Attempt {DesignRequestId} {ModuleId}", DesignRequestId, ModuleId);
            }
            return Task.CompletedTask;
        }

        // ... other service methods for Appliance, Detail, etc.
        public Task<List<Appliance>> GetAppliancesAsync(int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_applianceRepository.GetAppliances(moduleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Appliance Get Attempt {ModuleId}", moduleId);
                return null;
            }
        }

        public Task<Appliance> GetApplianceAsync(int applianceId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_applianceRepository.GetAppliance(applianceId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Appliance Get Attempt {ApplianceId} {ModuleId}", applianceId, moduleId);
                return null;
            }
        }

        public Task<Appliance> AddApplianceAsync(Appliance appliance)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, appliance.ModuleId, PermissionNames.Edit))
            {
                appliance = _applianceRepository.AddAppliance(appliance);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Appliance Added {Appliance}", appliance);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Appliance Add Attempt {Appliance}", appliance);
                appliance = null;
            }
            return Task.FromResult(appliance);
        }

        public Task<Appliance> UpdateApplianceAsync(Appliance appliance)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, appliance.ModuleId, PermissionNames.Edit))
            {
                appliance = _applianceRepository.UpdateAppliance(appliance);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Appliance Updated {Appliance}", appliance);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Appliance Update Attempt {Appliance}", appliance);
                appliance = null;
            }
            return Task.FromResult(appliance);
        }

        public Task DeleteApplianceAsync(int applianceId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _applianceRepository.DeleteAppliance(applianceId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Appliance Deleted {ApplianceId}", applianceId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Appliance Delete Attempt {ApplianceId} {ModuleId}", applianceId, moduleId);
            }
            return Task.CompletedTask;
        }

        // Detail Methods...
        public Task<List<Detail>> GetDetailsAsync(int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_detailRepository.GetDetails(moduleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Detail Get Attempt {ModuleId}", moduleId);
                return null;
            }
        }

        public Task<Detail> GetDetailAsync(int detailId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_detailRepository.GetDetail(detailId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Detail Get Attempt {DetailId} {ModuleId}", detailId, moduleId);
                return null;
            }
        }

        public Task<Detail> AddDetailAsync(Detail detail)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, detail.ModuleId, PermissionNames.Edit))
            {
                detail = _detailRepository.AddDetail(detail);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Detail Added {Detail}", detail);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Detail Add Attempt {Detail}", detail);
                detail = null;
            }
            return Task.FromResult(detail);
        }

        public Task<Detail> UpdateDetailAsync(Detail detail)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, detail.ModuleId, PermissionNames.Edit))
            {
                detail = _detailRepository.UpdateDetail(detail);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Detail Updated {Detail}", detail);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Detail Update Attempt {Detail}", detail);
                detail = null;
            }
            return Task.FromResult(detail);
        }

        public Task DeleteDetailAsync(int detailId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _detailRepository.DeleteDetail(detailId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Detail Deleted {DetailId}", detailId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Detail Delete Attempt {DetailId} {ModuleId}", detailId, moduleId);
            }
            return Task.CompletedTask;
        }

        // ApplianceToRequest Methods...
        public Task<List<ApplianceToRequest>> GetApplianceToRequestsAsync(int designRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_applianceToRequestRepository.GetApplianceToRequests(designRequestId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized ApplianceToRequest Get Attempt {DesignRequestId} {ModuleId}", designRequestId, moduleId);
                return null;
            }
        }

        public Task<ApplianceToRequest> GetApplianceToRequestAsync(int applianceToRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_applianceToRequestRepository.GetApplianceToRequest(applianceToRequestId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized ApplianceToRequest Get Attempt {ApplianceToRequestId} {ModuleId}", applianceToRequestId, moduleId);
                return null;
            }
        }

        public Task<ApplianceToRequest> AddApplianceToRequestAsync(ApplianceToRequest applianceToRequest)
        {
            // Permissions should be checked based on the associated DesignRequest's ModuleId
            var designRequest = _DesignRequestRepository.GetDesignRequest(applianceToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                applianceToRequest = _applianceToRequestRepository.AddApplianceToRequest(applianceToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "ApplianceToRequest Added {ApplianceToRequest}", applianceToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized ApplianceToRequest Add Attempt {ApplianceToRequest}", applianceToRequest);
                applianceToRequest = null;
            }
            return Task.FromResult(applianceToRequest);
        }

        public Task<ApplianceToRequest> UpdateApplianceToRequestAsync(ApplianceToRequest applianceToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(applianceToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                applianceToRequest = _applianceToRequestRepository.UpdateApplianceToRequest(applianceToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "ApplianceToRequest Updated {ApplianceToRequest}", applianceToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized ApplianceToRequest Update Attempt {ApplianceToRequest}", applianceToRequest);
                applianceToRequest = null;
            }
            return Task.FromResult(applianceToRequest);
        }

        public Task DeleteApplianceToRequestAsync(int applianceToRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _applianceToRequestRepository.DeleteApplianceToRequest(applianceToRequestId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "ApplianceToRequest Deleted {ApplianceToRequestId}", applianceToRequestId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized ApplianceToRequest Delete Attempt {ApplianceToRequestId} {ModuleId}", applianceToRequestId, moduleId);
            }
            return Task.CompletedTask;
        }

        // DetailToRequest Methods...
        public Task<List<DetailToRequest>> GetDetailToRequestsAsync(int designRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_detailToRequestRepository.GetDetailToRequests(designRequestId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DetailToRequest Get Attempt {DesignRequestId} {ModuleId}", designRequestId, moduleId);
                return null;
            }
        }

        public Task<DetailToRequest> GetDetailToRequestAsync(int detailToRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_detailToRequestRepository.GetDetailToRequest(detailToRequestId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DetailToRequest Get Attempt {DetailToRequestId} {ModuleId}", detailToRequestId, moduleId);
                return null;
            }
        }

        public Task<DetailToRequest> AddDetailToRequestAsync(DetailToRequest detailToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(detailToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                detailToRequest = _detailToRequestRepository.AddDetailToRequest(detailToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "DetailToRequest Added {DetailToRequest}", detailToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DetailToRequest Add Attempt {DetailToRequest}", detailToRequest);
                detailToRequest = null;
            }
            return Task.FromResult(detailToRequest);
        }

        public Task<DetailToRequest> UpdateDetailToRequestAsync(DetailToRequest detailToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(detailToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                detailToRequest = _detailToRequestRepository.UpdateDetailToRequest(detailToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "DetailToRequest Updated {DetailToRequest}", detailToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DetailToRequest Update Attempt {DetailToRequest}", detailToRequest);
                detailToRequest = null;
            }
            return Task.FromResult(detailToRequest);
        }

        public Task DeleteDetailToRequestAsync(int detailToRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _detailToRequestRepository.DeleteDetailToRequest(detailToRequestId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "DetailToRequest Deleted {DetailToRequestId}", detailToRequestId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized DetailToRequest Delete Attempt {DetailToRequestId} {ModuleId}", detailToRequestId, moduleId);
            }
            return Task.CompletedTask;
        }

        // NoteToRequest Methods
        public Task<List<NoteToRequest>> GetNoteToRequestsAsync(int designRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_noteToRequestRepository.GetNoteToRequests(designRequestId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NoteToRequest Get Attempt {DesignRequestId} {ModuleId}", designRequestId, moduleId);
                return null;
            }
        }

        public Task<NoteToRequest> GetNoteToRequestAsync(int noteId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_noteToRequestRepository.GetNoteToRequest(noteId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NoteToRequest Get Attempt {NoteId} {ModuleId}", noteId, moduleId);
                return null;
            }
        }

        public Task<NoteToRequest> AddNoteToRequestAsync(NoteToRequest noteToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(noteToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                noteToRequest = _noteToRequestRepository.AddNoteToRequest(noteToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "NoteToRequest Added {NoteToRequest}", noteToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NoteToRequest Add Attempt {NoteToRequest}", noteToRequest);
                noteToRequest = null;
            }
            return Task.FromResult(noteToRequest);
        }

        public Task<NoteToRequest> UpdateNoteToRequestAsync(NoteToRequest noteToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(noteToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                noteToRequest = _noteToRequestRepository.UpdateNoteToRequest(noteToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "NoteToRequest Updated {NoteToRequest}", noteToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NoteToRequest Update Attempt {NoteToRequest}", noteToRequest);
                noteToRequest = null;
            }
            return Task.FromResult(noteToRequest);
        }

        public Task DeleteNoteToRequestAsync(int noteId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _noteToRequestRepository.DeleteNoteToRequest(noteId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "NoteToRequest Deleted {NoteId}", noteId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NoteToRequest Delete Attempt {NoteId} {ModuleId}", noteId, moduleId);
            }
            return Task.CompletedTask;
        }

        // FileToRequest methods
        public Task<List<FileToRequest>> GetFileToRequestsAsync(int designRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_fileToRequestRepository.GetFileToRequests(designRequestId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FileToRequest Get Attempt {DesignRequestId} {ModuleId}", designRequestId, moduleId);
                return null;
            }
        }

        public Task<FileToRequest> GetFileToRequestAsync(int fileToRequestId, int moduleId)
        {
            var fileToRequest = _fileToRequestRepository.GetFileToRequest(fileToRequestId);
            if (fileToRequest != null)
            {
                var designRequest = _DesignRequestRepository.GetDesignRequest(fileToRequest.DesignRequestId);
                if (designRequest != null && designRequest.ModuleId == moduleId && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
                {
                    return Task.FromResult(fileToRequest);
                }
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FileToRequest Get Attempt {FileToRequestId} {ModuleId}", fileToRequestId, moduleId);
            return Task.FromResult<FileToRequest>(null);
        }

        public Task<FileToRequest> AddFileToRequestAsync(FileToRequest fileToRequest)
        {
            // Public form, no permission check needed to add
            fileToRequest = _fileToRequestRepository.AddFileToRequest(fileToRequest);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "FileToRequest Added {FileToRequest}", fileToRequest);
            return Task.FromResult(fileToRequest);
        }

        public Task<FileToRequest> UpdateFileToRequestAsync(FileToRequest fileToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(fileToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                fileToRequest = _fileToRequestRepository.UpdateFileToRequest(fileToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "FileToRequest Updated {FileToRequest}", fileToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FileToRequest Update Attempt {FileToRequest}", fileToRequest);
                fileToRequest = null;
            }
            return Task.FromResult(fileToRequest);
        }

        public Task DeleteFileToRequestAsync(int fileToRequestId, int moduleId)
        {
            var fileToRequest = _fileToRequestRepository.GetFileToRequest(fileToRequestId);
            if (fileToRequest != null)
            {
                var designRequest = _DesignRequestRepository.GetDesignRequest(fileToRequest.DesignRequestId);
                if (designRequest != null && designRequest.ModuleId == moduleId && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
                {
                    _fileToRequestRepository.DeleteFileToRequest(fileToRequestId);
                    _logger.Log(LogLevel.Information, this, LogFunction.Delete, "FileToRequest Deleted {FileToRequestId}", fileToRequestId);
                }
                else
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FileToRequest Delete Attempt {FileToRequestId} {ModuleId}", fileToRequestId, moduleId);
                }
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FileToRequest Delete Attempt on non-existent file {FileToRequestId} {ModuleId}", fileToRequestId, moduleId);
            }
            return Task.CompletedTask;
        }
    }
}