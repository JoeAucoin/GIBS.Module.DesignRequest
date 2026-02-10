using GIBS.Module.DesignRequest.Models;
using GIBS.Module.DesignRequest.Repository;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Security;
using Oqtane.Services;
using Oqtane.Shared;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;



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
        private readonly INotificationToRequestRepository _notificationToRequestRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;
        private readonly INotificationRepository _notifications;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleService;
        private readonly ISettingRepository _settings;


        public ServerDesignRequestService(IDesignRequestRepository DesignRequestRepository, INotificationToRequestRepository notificationToRequestRepository, IUserRoleRepository userRoleService, IApplianceRepository applianceRepository, IDetailRepository detailRepository, IApplianceToRequestRepository applianceToRequestRepository, IDetailToRequestRepository detailToRequestRepository, INoteToRequestRepository noteToRequestRepository, IFileToRequestRepository fileToRequestRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor, INotificationRepository notifications, ISettingRepository settings, IUserRepository userRepository)
        {
            _DesignRequestRepository = DesignRequestRepository;
            _applianceRepository = applianceRepository;
            _detailRepository = detailRepository;
            _applianceToRequestRepository = applianceToRequestRepository;
            _detailToRequestRepository = detailToRequestRepository;
            _noteToRequestRepository = noteToRequestRepository;
            _fileToRequestRepository = fileToRequestRepository;
            _notificationToRequestRepository = notificationToRequestRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
            _notifications = notifications;
            _userRepository = userRepository;
            _userRoleService = userRoleService;
            _settings = settings;
        }

        private (string PayPalPayee, string ClientId, string ClientSecret, PaypalServerSdk.Standard.Environment Environment) GetPayPalCredentials(int moduleId)
        {
            var settings = _settings.GetSettings("Module", moduleId).ToDictionary(s => s.SettingName, s => s.SettingValue);
            var isSandbox = bool.Parse(settings.GetValueOrDefault("PayPalSandboxMode", "true"));

            if (isSandbox)
            {
                return (
                    settings.GetValueOrDefault("PayPalSandboxPayee"),
                    settings.GetValueOrDefault("PayPalSandboxClientId"),
                    settings.GetValueOrDefault("PayPalSandboxClientSecret"),
                    PaypalServerSdk.Standard.Environment.Sandbox
                );
            }
            else
            {
                return (
                    settings.GetValueOrDefault("PayPalPayee"),
                    settings.GetValueOrDefault("OAuthClientId"),
                    settings.GetValueOrDefault("OAuthClientSecret"),
                    PaypalServerSdk.Standard.Environment.Production
                );
            }
        }



        public async Task<PayPalOrderResponseDto> CreatePayPalOrderAsync(Models.PaymentRecord paymentRecord)
        {
            var (payPalPayee, clientId, clientSecret, environment) = GetPayPalCredentials(paymentRecord.ModuleId);

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "PayPal client ID or secret is not configured for module {ModuleId}", paymentRecord.ModuleId);
                throw new System.Exception("PayPal is not configured.");
            }

            var payPalClient = new PaypalServerSdkClient.Builder()
                .ClientCredentialsAuth(new ClientCredentialsAuthModel.Builder(clientId, clientSecret).Build())
                .Environment(environment)
                .Build();

            var ordersController = payPalClient.OrdersController;

            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, paymentRecord.ModuleId, PermissionNames.View))
            {
                try
                {
                    var orderRequest = new OrderRequest
                    {
                        Intent = CheckoutPaymentIntent.Capture,
                        PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        Description = $"Design Request Credits for {paymentRecord.PurchaserName ?? "Design Request Credits"}",
                        InvoiceId = "MID" + paymentRecord.ModuleId.ToString() + "-UID" + paymentRecord.UserId.ToString() + "-RID" + paymentRecord.PaymentId.ToString(),

                        Amount = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            MValue = paymentRecord.Amount.ToString("F2")
                        },
                        Payee = new PayeeBase
                        {
                          //  EmailAddress = payPalPayee.ToString() 
                            EmailAddress = "joe-facilitator@gibs.com"
                        }
                    }
                },

                        Payer = new PaypalServerSdk.Standard.Models.Payer
                        {
                            Name = new Name
                            {
                                GivenName = paymentRecord.PurchaserName ?? "Customer"
                            },
                            EmailAddress = paymentRecord.EmailAddress
                        }
                    };

                    // Log the request payload
                    _logger.Log(LogLevel.Information, this, LogFunction.Create, "PayPal Order Request: {PayPalRequest}", JsonSerializer.Serialize(orderRequest));

                    var createOrderInput = new CreateOrderInput { Body = orderRequest };
                    var response = await ordersController.CreateOrderAsync(createOrderInput);

                    var fullOrder = response.Data;
                    return new PayPalOrderResponseDto
                    {
                        OrderId = fullOrder.Id,
                        RawOrderJson = JsonSerializer.Serialize(fullOrder)
                    };
                }
                catch (System.Exception ex)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, ex, "Error Creating PayPal Order {Error}", ex.Message);
                    throw;
                }
            }

            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PayPal Order Create Attempt {PaymentRecord}", paymentRecord);
            return null;
        }

        public async Task<string> CapturePayPalOrderAsync(string orderId, int moduleId)
        {
            var (payPalPayee, clientId, clientSecret, environment) = GetPayPalCredentials(moduleId);

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "PayPal client ID or secret is not configured for module {ModuleId}", moduleId);
                throw new System.Exception("PayPal is not configured.");
            }

            var payPalClient = new PaypalServerSdkClient.Builder()
                .ClientCredentialsAuth(new ClientCredentialsAuthModel.Builder(clientId, clientSecret).Build())
                .Environment(environment)
                .Build();

            var ordersController = payPalClient.OrdersController;

            // Authorization is already handled by the controller, but we accept moduleId to match the interface
            try
            {
                var captureOrderInput = new CaptureOrderInput { Id = orderId };
                var response = await ordersController.CaptureOrderAsync(captureOrderInput);
                var capturedOrder = response.Data;

                // Serialize the entire captured order object to a JSON string
                return JsonSerializer.Serialize(capturedOrder);
            }
            catch (System.Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, ex, "Error Capturing PayPal Order {Error}", ex.Message);
                throw;
            }
        }


        public async Task SendHtmlEmailAsync(string recipientName, string recipientEmail, string bccName, string bccEmail, string replyToName, string replyToEmail, string subject, string htmlMessage)
        {
            // Retrieve Site Settings
            var settings = _settings.GetSettings(EntityNames.Site, _alias.SiteId, EntityNames.Host, -1).ToList();

            string GetSetting(string key, string defaultValue) =>
                settings.FirstOrDefault(s => s.SettingName == key)?.SettingValue ?? defaultValue;

            string smtpHost = GetSetting("SMTPHost", "");
            int smtpPort = int.Parse(GetSetting("SMTPPort", "587"));
            string smtpUserName = GetSetting("SMTPUsername", "");
            string smtpPassword = GetSetting("SMTPPassword", "");
            string smtpSender = GetSetting("SMTPSender", smtpUserName);
            string smtpSSL = GetSetting("SMTPSSL", "false"); // Oqtane often has this setting

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Webmaster", smtpSender));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.ReplyTo.Add(new MailboxAddress(replyToName, replyToEmail));

            if (!string.IsNullOrEmpty(bccEmail))
            {
                message.Bcc.Add(new MailboxAddress(bccName, bccEmail));
            }

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage,
                TextBody = "Please view this email in a client that supports HTML."
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = false;

            // Connect
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.Auto);

            // Authenticate
            if (!string.IsNullOrEmpty(smtpUserName) && !string.IsNullOrEmpty(smtpPassword))
            {
                await client.AuthenticateAsync(smtpUserName, smtpPassword);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }


        // Other service methods...
        public Task<List<User>> GetUsersAsync()
        {
            // use the server-side IUserRepository to get the users
            var users = _userRepository.GetUsers().ToList();
            users = users.FindAll(u => u.IsDeleted == false); // Filter out deleted users
            return Task.FromResult(users);
        }

        //GetUsersByRoleAsync
        public Task<List<User>> GetUsersByRoleAsync(int siteId, string roleName)
        {
            List<User> usersInRole = new List<User>();

            // Retrieve UserRole objects for the given site and role name
            List<UserRole> userRoles = (List<UserRole>)_userRoleService.GetUserRoles(roleName, siteId);

            // Extract the User objects from the UserRole objects
            foreach (UserRole userRole in userRoles)
            {
                usersInRole.Add(userRole.User);
            }

            return Task.FromResult(usersInRole);
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

        public Task<Paged<Models.DesignRequest>> GetDesignRequestsAsync(int moduleId, int page, int pageSize)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                var data = _DesignRequestRepository.GetDesignRequests(moduleId, page, pageSize);
                var totalCount = _DesignRequestRepository.CountDesignRequests(moduleId);
                var pagedResult = new Paged<Models.DesignRequest> { Data = data.ToList(), TotalCount = totalCount };
                return Task.FromResult(pagedResult);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Paged DesignRequest Get Attempt {ModuleId}", moduleId);
                return Task.FromResult<Paged<Models.DesignRequest>>(null);
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
                notification = _notifications.AddNotification(notification);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Notification Added", notification);

                // Log notification to history
                var notificationToRequest = new NotificationToRequest
                {
                    DesignRequestId = savedDesignRequest.DesignRequestId,
                    NotificationId = notification.NotificationId,
                    FromUserId = notification.FromUserId ?? -1,
                    FromDisplayName = notification.FromDisplayName,
                    FromEmail = notification.FromEmail,
                    ToUserId = notification.ToUserId ?? -1,
                    ToDisplayName = notification.ToDisplayName,
                    ToEmail = notification.ToEmail,
                    Subject = notification.Subject,
                    Body = notification.Body
                };
                _notificationToRequestRepository.AddNotificationToRequest(notificationToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Notification To Request History Added {NotificationToRequest}", notificationToRequest);
            }

            return await Task.FromResult(savedDesignRequest);
        }

        public Task<Models.DesignRequest> UpdateDesignRequestAsync(Models.DesignRequest DesignRequest)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, DesignRequest.ModuleId, PermissionNames.View))
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

        // Appliance Methods
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
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, appliance.ModuleId, PermissionNames.View))
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

        // Detail Methods
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

        // ApplianceToRequest Methods
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
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.View))
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
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.View))
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

        // DetailToRequest Methods
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
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.View))
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
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.View))
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
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.View))
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

        // NotificationToRequest Methods
        public Task<List<NotificationToRequest>> GetNotificationToRequestsAsync(int designRequestId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_notificationToRequestRepository.GetNotificationToRequests(designRequestId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NotificationToRequest Get Attempt {DesignRequestId} {ModuleId}", designRequestId, moduleId);
                return null;
            }
        }

        public Task<NotificationToRequest> GetNotificationToRequestAsync(int notificationToRequestId, int moduleId)
        {
            var notificationToRequest = _notificationToRequestRepository.GetNotificationToRequest(notificationToRequestId, false);
            if (notificationToRequest != null)
            {
                var designRequest = _DesignRequestRepository.GetDesignRequest(notificationToRequest.DesignRequestId);
                if (designRequest != null && designRequest.ModuleId == moduleId && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
                {
                    return Task.FromResult(notificationToRequest);
                }
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NotificationToRequest Get Attempt {NotificationToRequestId} {ModuleId}", notificationToRequestId, moduleId);
            return Task.FromResult<NotificationToRequest>(null);
        }

        public Task<NotificationToRequest> AddNotificationToRequestAsync(NotificationToRequest notificationToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(notificationToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.View))
            {
                notificationToRequest = _notificationToRequestRepository.AddNotificationToRequest(notificationToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "NotificationToRequest Added {NotificationToRequest}", notificationToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NotificationToRequest Add Attempt {NotificationToRequest}", notificationToRequest);
                notificationToRequest = null;
            }
            return Task.FromResult(notificationToRequest);
        }

        public Task<NotificationToRequest> UpdateNotificationToRequestAsync(NotificationToRequest notificationToRequest)
        {
            var designRequest = _DesignRequestRepository.GetDesignRequest(notificationToRequest.DesignRequestId);
            if (designRequest != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, designRequest.ModuleId, PermissionNames.Edit))
            {
                notificationToRequest = _notificationToRequestRepository.UpdateNotificationToRequest(notificationToRequest);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "NotificationToRequest Updated {NotificationToRequest}", notificationToRequest);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NotificationToRequest Update Attempt {NotificationToRequest}", notificationToRequest);
                notificationToRequest = null;
            }
            return Task.FromResult(notificationToRequest);
        }

        public Task DeleteNotificationToRequestAsync(int notificationToRequestId, int moduleId)
        {
            var notificationToRequest = _notificationToRequestRepository.GetNotificationToRequest(notificationToRequestId, false);
            if (notificationToRequest != null)
            {
                var designRequest = _DesignRequestRepository.GetDesignRequest(notificationToRequest.DesignRequestId);
                if (designRequest != null && designRequest.ModuleId == moduleId && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
                {
                    _notificationToRequestRepository.DeleteNotificationToRequest(notificationToRequestId);
                    _logger.Log(LogLevel.Information, this, LogFunction.Delete, "NotificationToRequest Deleted {NotificationToRequestId}", notificationToRequestId);
                }
                else
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NotificationToRequest Delete Attempt {NotificationToRequestId} {ModuleId}", notificationToRequestId, moduleId);
                }
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized NotificationToRequest Delete Attempt on non-existent item {NotificationToRequestId} {ModuleId}", notificationToRequestId, moduleId);
            }
            return Task.CompletedTask;
        }

        // UserCredits Methods
        public Task<List<UserCredit>> GetUserCreditsAsync(int ModuleId)
        {
            //if (_accessor.HttpContext.User.IsInRole(RoleNames.Admin))
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                return Task.FromResult(_DesignRequestRepository.GetUserCredits(ModuleId).ToList());
            }
            else
            {
                return Task.FromResult(new List<UserCredit>());
            }
        }

        public Task<UserCredit> GetUserCreditAsync(int UserCreditId, int ModuleId)
        {
            var userCredit = _DesignRequestRepository.GetUserCredit(UserCreditId);
            if (userCredit != null && userCredit.ModuleId == ModuleId)
            {
                return Task.FromResult(userCredit);
            }
            return Task.FromResult<UserCredit>(null);
        }

        public Task<UserCredit> GetUserCreditByUserAsync(int ModuleId, int UserId)
        {
            return Task.FromResult(_DesignRequestRepository.GetUserCreditByUser(ModuleId, UserId));
        }

        public Task<UserCredit> AddUserCreditAsync(UserCredit UserCredit)
        {
            return Task.FromResult(_DesignRequestRepository.AddUserCredit(UserCredit));
        }

        public Task<UserCredit> UpdateUserCreditAsync(UserCredit UserCredit)
        {
            return Task.FromResult(_DesignRequestRepository.UpdateUserCredit(UserCredit));
        }

        public Task DeleteUserCreditAsync(int UserCreditId, int ModuleId)
        {
            _DesignRequestRepository.DeleteUserCredit(UserCreditId);
            return Task.CompletedTask;
        }

        // CreditPackage methods
        public Task<List<CreditPackage>> GetCreditPackagesAsync(int ModuleId)
        {
            return Task.FromResult(_DesignRequestRepository.GetCreditPackages(ModuleId).ToList());
        }

        public Task<CreditPackage> GetCreditPackageAsync(int CreditPackageId, int ModuleId)
        {
            var package = _DesignRequestRepository.GetCreditPackage(CreditPackageId);
            if (package != null && package.ModuleId == ModuleId)
            {
                return Task.FromResult(package);
            }
            return Task.FromResult<CreditPackage>(null);
        }

        public Task<CreditPackage> AddCreditPackageAsync(CreditPackage CreditPackage)
        {
            return Task.FromResult(_DesignRequestRepository.AddCreditPackage(CreditPackage));
        }

        public Task<CreditPackage> UpdateCreditPackageAsync(CreditPackage CreditPackage)
        {
            return Task.FromResult(_DesignRequestRepository.UpdateCreditPackage(CreditPackage));
        }

        public Task DeleteCreditPackageAsync(int CreditPackageId, int ModuleId)
        {
            _DesignRequestRepository.DeleteCreditPackage(CreditPackageId);
            return Task.CompletedTask;
        }

        // CreditTransaction methods
        public Task<List<CreditTransaction>> GetCreditTransactionsAsync(int ModuleId)
        {
            return Task.FromResult(_DesignRequestRepository.GetCreditTransactions(ModuleId).ToList());
        }

        public Task<List<CreditTransaction>> GetCreditTransactionsByUserAsync(int ModuleId, int UserId)
        {
            return Task.FromResult(_DesignRequestRepository.GetCreditTransactionsByUser(ModuleId, UserId).ToList());
        }

        public Task<CreditTransaction> GetCreditTransactionAsync(int TransactionId, int ModuleId)
        {
            return Task.FromResult(_DesignRequestRepository.GetCreditTransaction(TransactionId));
        }

        public Task<CreditTransaction> AddCreditTransactionAsync(CreditTransaction CreditTransaction, int ModuleId)
        {
            // Note: Updated to match interface which likely just passes model in future refactor, but kept compatible
            return Task.FromResult(_DesignRequestRepository.AddCreditTransaction(CreditTransaction));
        }

        // Overload to match interface if needed, or separate method. 
        // Based on previous step, I defined only AddCreditTransactionAsync(CreditTransaction) in interface, 
        // but here it was implemented with ModuleId.
        public Task<CreditTransaction> AddCreditTransactionAsync(CreditTransaction CreditTransaction)
        {
            return Task.FromResult(_DesignRequestRepository.AddCreditTransaction(CreditTransaction));
        }

        public Task<CreditTransaction> UpdateCreditTransactionAsync(CreditTransaction CreditTransaction, int ModuleId)
        {
            return Task.FromResult(_DesignRequestRepository.UpdateCreditTransaction(CreditTransaction));
        }

        public Task DeleteCreditTransactionAsync(int TransactionId, int ModuleId)
        {
            _DesignRequestRepository.DeleteCreditTransaction(TransactionId);
            return Task.CompletedTask;
        }

        // PaymentRecord Methods
        public Task<List<PaymentRecord>> GetPaymentRecordsAsync(int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_DesignRequestRepository.GetPaymentRecords(moduleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PaymentRecord Get Attempt {ModuleId}", moduleId);
                return null;
            }
        }

        public Task<List<PaymentRecord>> GetPaymentRecordsByUserAsync(int moduleId, int userId)
        {
            var user = _accessor.HttpContext.User;
            var currentUserIdStr = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (user.IsInRole(RoleNames.Admin) ||
                user.IsInRole(RoleNames.Host) ||
                (currentUserIdStr != null && currentUserIdStr == userId.ToString()))
            {
                return Task.FromResult(_DesignRequestRepository.GetPaymentRecordsByUser(moduleId, userId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PaymentRecord Get By User Attempt {ModuleId} {UserId}", moduleId, userId);
                return null;
            }
        }

        public Task<PaymentRecord> GetPaymentRecordAsync(int paymentId, int moduleId)
        {
            var payment = _DesignRequestRepository.GetPaymentRecord(paymentId);
            if (payment != null)
            {
                // Basic view check. For strict security, check ownership in caller or here if needed.
                if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
                {
                    return Task.FromResult(payment);
                }
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PaymentRecord Get Attempt {PaymentId} {ModuleId}", paymentId, moduleId);
            return Task.FromResult<PaymentRecord>(null);
        }

        public Task<PaymentRecord> AddPaymentRecordAsync(PaymentRecord paymentRecord)
        {
            // Typically Admin or System adding payments, or callbacks. 
            // If public callback handles param, checks might differ.
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, paymentRecord.ModuleId, PermissionNames.View) ||
                _accessor.HttpContext.User.IsInRole(RoleNames.Admin))
            {
                return Task.FromResult(_DesignRequestRepository.AddPaymentRecord(paymentRecord));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PaymentRecord Add Attempt {PaymentRecord}", paymentRecord);
                return Task.FromResult<PaymentRecord>(null);
            }
        }

        public Task<PaymentRecord> UpdatePaymentRecordAsync(PaymentRecord paymentRecord)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, paymentRecord.ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_DesignRequestRepository.UpdatePaymentRecord(paymentRecord));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PaymentRecord Update Attempt {PaymentRecord}", paymentRecord);
                return Task.FromResult<PaymentRecord>(null);
            }
        }

        public Task DeletePaymentRecordAsync(int paymentId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _DesignRequestRepository.DeletePaymentRecord(paymentId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "PaymentRecord Deleted {PaymentId}", paymentId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PaymentRecord Delete Attempt {PaymentId} {ModuleId}", paymentId, moduleId);
            }
            return Task.CompletedTask;
        }
    }
}