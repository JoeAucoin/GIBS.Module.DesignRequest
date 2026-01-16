using GIBS.Module.DesignRequest.Models;
using Oqtane.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GIBS.Module.DesignRequest.Services
{
    public class DesignRequestService : ServiceBase, IDesignRequestService
    {
        public DesignRequestService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("DesignRequest");

        public async Task<List<Models.DesignRequest>> GetDesignRequestsAsync(int moduleId)
        {
            return await GetJsonAsync<List<Models.DesignRequest>>($"{Apiurl}?moduleid={moduleId}");
        }

        public async Task<Paged<Models.DesignRequest>> GetDesignRequestsAsync(int moduleId, int page, int pageSize)
        {
            return await GetJsonAsync<Paged<Models.DesignRequest>>($"{Apiurl}/paged?moduleid={moduleId}&page={page}&pagesize={pageSize}");
        }

        public async Task<Models.DesignRequest> GetDesignRequestAsync(int DesignRequestId, int ModuleId)
        {
            return await GetJsonAsync<Models.DesignRequest>($"{Apiurl}/{DesignRequestId}?moduleid={ModuleId}");
        }

        public async Task<Models.DesignRequest> AddDesignRequestAsync(Models.DesignRequest DesignRequest)
        {
            return await PostJsonAsync<Models.DesignRequest>($"{Apiurl}?moduleid={DesignRequest.ModuleId}", DesignRequest);
        }

        public async Task<Models.DesignRequest> UpdateDesignRequestAsync(Models.DesignRequest DesignRequest)
        {
            return await PutJsonAsync<Models.DesignRequest>($"{Apiurl}/{DesignRequest.DesignRequestId}?moduleid={DesignRequest.ModuleId}", DesignRequest);
        }

        public async Task DeleteDesignRequestAsync(int DesignRequestId, int ModuleId)
        {
            await DeleteAsync($"{Apiurl}/{DesignRequestId}?moduleid={ModuleId}");
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await GetJsonAsync<List<User>>($"{Apiurl}/users");
        }
        //GetUsersByRoleAsync
        public async Task<List<User>> GetUsersByRoleAsync(int siteId, string roleName)
        {
            // Call the UserRole API to get user-role assignments for the specified role
            var userRoles = await GetJsonAsync<List<UserRole>>($"/api/UserRole?siteid={siteId}&rolename={roleName}");
            // Extract the User objects from the UserRole results
            return userRoles?.Select(ur => ur.User).Where(u => u != null).ToList() ?? new List<User>();
        }

        // Appliance Methods
        private string ApplianceApiurl => CreateApiUrl("Appliance");

        public async Task<List<Appliance>> GetAppliancesAsync(int moduleId)
        {
            return await GetJsonAsync<List<Appliance>>($"{ApplianceApiurl}?moduleid={moduleId}");
        }

        public async Task<Appliance> GetApplianceAsync(int applianceId, int moduleId)
        {
            return await GetJsonAsync<Appliance>($"{ApplianceApiurl}/{applianceId}?moduleid={moduleId}");
        }

        public async Task<Appliance> AddApplianceAsync(Appliance appliance)
        {
            return await PostJsonAsync<Appliance>($"{ApplianceApiurl}?moduleid={appliance.ModuleId}", appliance);
        }

        public async Task<Appliance> UpdateApplianceAsync(Appliance appliance)
        {
            return await PutJsonAsync<Appliance>($"{ApplianceApiurl}/{appliance.ApplianceId}?moduleid={appliance.ModuleId}", appliance);
        }

        public async Task DeleteApplianceAsync(int applianceId, int moduleId)
        {
            await DeleteAsync($"{ApplianceApiurl}/{applianceId}?moduleid={moduleId}");
        }

        // Detail Methods
        private string DetailApiurl => CreateApiUrl("Detail");

        public async Task<List<Detail>> GetDetailsAsync(int moduleId)
        {
            return await GetJsonAsync<List<Detail>>($"{DetailApiurl}?moduleid={moduleId}");
        }

        public async Task<Detail> GetDetailAsync(int detailId, int moduleId)
        {
            return await GetJsonAsync<Detail>($"{DetailApiurl}/{detailId}?moduleid={moduleId}");
        }

        public async Task<Detail> AddDetailAsync(Detail detail)
        {
            return await PostJsonAsync<Detail>($"{DetailApiurl}?moduleid={detail.ModuleId}", detail);
        }

        public async Task<Detail> UpdateDetailAsync(Detail detail)
        {
            return await PutJsonAsync<Detail>($"{DetailApiurl}/{detail.DetailId}?moduleid={detail.ModuleId}", detail);
        }

        public async Task DeleteDetailAsync(int detailId, int moduleId)
        {
            await DeleteAsync($"{DetailApiurl}/{detailId}?moduleid={moduleId}");
        }

        // ApplianceToRequest Methods
        private string ApplianceToRequestApiurl => CreateApiUrl("ApplianceToRequest");

        public async Task<List<ApplianceToRequest>> GetApplianceToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<ApplianceToRequest>>($"{ApplianceToRequestApiurl}/GetByDesignRequest/{designRequestId}?moduleid={moduleId}");
        }

        public async Task<ApplianceToRequest> GetApplianceToRequestAsync(int applianceToRequestId, int moduleId)
        {
            return await GetJsonAsync<ApplianceToRequest>($"{ApplianceToRequestApiurl}/{applianceToRequestId}?moduleid={moduleId}");
        }

        public async Task<ApplianceToRequest> AddApplianceToRequestAsync(ApplianceToRequest applianceToRequest)
        {
            var designRequest = await GetDesignRequestAsync(applianceToRequest.DesignRequestId, -1);
            return await PostJsonAsync<ApplianceToRequest>($"{ApplianceToRequestApiurl}?moduleid={designRequest.ModuleId}", applianceToRequest);
        }

        public async Task<ApplianceToRequest> UpdateApplianceToRequestAsync(ApplianceToRequest applianceToRequest)
        {
            var designRequest = await GetDesignRequestAsync(applianceToRequest.DesignRequestId, -1);
            return await PutJsonAsync<ApplianceToRequest>($"{ApplianceToRequestApiurl}/{applianceToRequest.ApplianceToRequestId}?moduleid={designRequest.ModuleId}", applianceToRequest);
        }

        public async Task DeleteApplianceToRequestAsync(int applianceToRequestId, int moduleId)
        {
            await DeleteAsync($"{ApplianceToRequestApiurl}/{applianceToRequestId}?moduleid={moduleId}");
        }

        // DetailToRequest Methods
        private string DetailToRequestApiurl => CreateApiUrl("DetailToRequest");

        public async Task<List<DetailToRequest>> GetDetailToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<DetailToRequest>>($"{DetailToRequestApiurl}/GetByDesignRequest/{designRequestId}?moduleid={moduleId}");
        }

        public async Task<DetailToRequest> GetDetailToRequestAsync(int detailToRequestId, int moduleId)
        {
            return await GetJsonAsync<DetailToRequest>($"{DetailToRequestApiurl}/{detailToRequestId}?moduleid={moduleId}");
        }

        public async Task<DetailToRequest> AddDetailToRequestAsync(DetailToRequest detailToRequest)
        {
            var designRequest = await GetDesignRequestAsync(detailToRequest.DesignRequestId, -1);
            return await PostJsonAsync<DetailToRequest>($"{DetailToRequestApiurl}?moduleid={designRequest.ModuleId}", detailToRequest);
        }

        public async Task<DetailToRequest> UpdateDetailToRequestAsync(DetailToRequest detailToRequest)
        {
            var designRequest = await GetDesignRequestAsync(detailToRequest.DesignRequestId, -1);
            return await PutJsonAsync<DetailToRequest>($"{DetailToRequestApiurl}/{detailToRequest.DetailToRequestId}?moduleid={designRequest.ModuleId}", detailToRequest);
        }

        public async Task DeleteDetailToRequestAsync(int detailToRequestId, int moduleId)
        {
            await DeleteAsync($"{DetailToRequestApiurl}/{detailToRequestId}?moduleid={moduleId}");
        }

        // NoteToRequest Methods
        private string NoteToRequestApiurl => CreateApiUrl("NoteToRequest");

        public async Task<List<NoteToRequest>> GetNoteToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<NoteToRequest>>($"{NoteToRequestApiurl}/GetByDesignRequest/{designRequestId}?moduleid={moduleId}");
        }

        public async Task<NoteToRequest> GetNoteToRequestAsync(int noteId, int moduleId)
        {
            return await GetJsonAsync<NoteToRequest>($"{NoteToRequestApiurl}/{noteId}?moduleid={moduleId}");
        }

        public async Task<NoteToRequest> AddNoteToRequestAsync(NoteToRequest noteToRequest)
        {
            var designRequest = await GetDesignRequestAsync(noteToRequest.DesignRequestId, -1);
            return await PostJsonAsync<NoteToRequest>($"{NoteToRequestApiurl}?moduleid={designRequest.ModuleId}", noteToRequest);
        }

        public async Task<NoteToRequest> UpdateNoteToRequestAsync(NoteToRequest noteToRequest)
        {
            var designRequest = await GetDesignRequestAsync(noteToRequest.DesignRequestId, -1);
            return await PutJsonAsync<NoteToRequest>($"{NoteToRequestApiurl}/{noteToRequest.NoteId}?moduleid={designRequest.ModuleId}", noteToRequest);
        }

        public async Task DeleteNoteToRequestAsync(int noteId, int moduleId)
        {
            await DeleteAsync($"{NoteToRequestApiurl}/{noteId}?moduleid={moduleId}");
        }

        // FileToRequest Methods
        private string FileToRequestApiurl => CreateApiUrl("FileToRequest");

        public async Task<List<FileToRequest>> GetFileToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<FileToRequest>>($"{FileToRequestApiurl}/GetByDesignRequest/{designRequestId}?moduleid={moduleId}");
        }

        public async Task<FileToRequest> GetFileToRequestAsync(int fileToRequestId, int moduleId)
        {
            return await GetJsonAsync<FileToRequest>($"{FileToRequestApiurl}/{fileToRequestId}?moduleid={moduleId}");
        }

        public async Task<FileToRequest> AddFileToRequestAsync(FileToRequest fileToRequest)
        {
            // The controller allows anonymous posts, so no authorization policy URL is strictly needed,
            // but we can still pass the module ID for consistency if needed in the future.
            var designRequest = await GetDesignRequestAsync(fileToRequest.DesignRequestId, -1);
            return await PostJsonAsync<FileToRequest>($"{FileToRequestApiurl}?moduleid={designRequest.ModuleId}", fileToRequest);
        }

        public async Task<FileToRequest> UpdateFileToRequestAsync(FileToRequest fileToRequest)
        {
            var designRequest = await GetDesignRequestAsync(fileToRequest.DesignRequestId, -1);
            return await PutJsonAsync<FileToRequest>($"{FileToRequestApiurl}/{fileToRequest.FileToRequestId}?moduleid={designRequest.ModuleId}", fileToRequest);
        }

        public async Task DeleteFileToRequestAsync(int fileToRequestId, int moduleId)
        {
            await DeleteAsync($"{FileToRequestApiurl}/{fileToRequestId}?moduleid={moduleId}");
        }

        // NotificationToRequest methods
        private string NotificationToRequestApiurl => CreateApiUrl("NotificationToRequest");

        public async Task<List<NotificationToRequest>> GetNotificationToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<NotificationToRequest>>($"{NotificationToRequestApiurl}/GetByDesignRequest/{designRequestId}?moduleid={moduleId}");
        }

        public async Task<NotificationToRequest> GetNotificationToRequestAsync(int notificationToRequestId, int moduleId)
        {
            return await GetJsonAsync<NotificationToRequest>($"{NotificationToRequestApiurl}/{notificationToRequestId}?moduleid={moduleId}");
        }

        public async Task<NotificationToRequest> AddNotificationToRequestAsync(NotificationToRequest notificationToRequest)
        {
            var designRequest = await GetDesignRequestAsync(notificationToRequest.DesignRequestId, -1);
            return await PostJsonAsync<NotificationToRequest>($"{NotificationToRequestApiurl}?moduleid={designRequest.ModuleId}", notificationToRequest);
        }

        public async Task<NotificationToRequest> UpdateNotificationToRequestAsync(NotificationToRequest notificationToRequest)
        {
            var designRequest = await GetDesignRequestAsync(notificationToRequest.DesignRequestId, -1);
            return await PutJsonAsync<NotificationToRequest>($"{NotificationToRequestApiurl}/{notificationToRequest.NotificationToRequestId}?moduleid={designRequest.ModuleId}", notificationToRequest);
        }

        public async Task DeleteNotificationToRequestAsync(int notificationToRequestId, int moduleId)
        {
            await DeleteAsync($"{NotificationToRequestApiurl}/{notificationToRequestId}?moduleid={moduleId}");
        }

        //UserCredits Methods
        //UserCredits Methods
        private string UserCreditApiurl => CreateApiUrl("UserCredit");

        public async Task<List<UserCredit>> GetUserCreditsAsync(int ModuleId)
        {
            // CHANGE: Used UserCreditApiurl instead of Apiurl
            List<UserCredit> userCredits = await GetJsonAsync<List<UserCredit>>(CreateAuthorizationPolicyUrl($"{UserCreditApiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
            return userCredits.OrderBy(item => item.CreatedOn).ToList();
        }

        public async Task<UserCredit> GetUserCreditAsync(int UserCreditId, int ModuleId)
        {
            // CHANGE: Used UserCreditApiurl instead of Apiurl
            return await GetJsonAsync<UserCredit>(CreateAuthorizationPolicyUrl($"{UserCreditApiurl}/{UserCreditId}", EntityNames.Module, ModuleId));
        }

        public async Task<UserCredit> GetUserCreditByUserAsync(int ModuleId, int UserId)
        {
            // CHANGE: Used UserCreditApiurl instead of Apiurl
            return await GetJsonAsync<UserCredit>(CreateAuthorizationPolicyUrl($"{UserCreditApiurl}/user/{UserId}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<UserCredit> AddUserCreditAsync(UserCredit UserCredit)
        {
            // CHANGE: Used UserCreditApiurl instead of Apiurl
            return await PostJsonAsync<UserCredit>(CreateAuthorizationPolicyUrl($"{UserCreditApiurl}", EntityNames.Module, UserCredit.ModuleId), UserCredit);
        }

        public async Task<UserCredit> UpdateUserCreditAsync(UserCredit UserCredit)
        {
            // CHANGE: Used UserCreditApiurl instead of Apiurl
            return await PutJsonAsync<UserCredit>(CreateAuthorizationPolicyUrl($"{UserCreditApiurl}/{UserCredit.UserCreditId}", EntityNames.Module, UserCredit.ModuleId), UserCredit);
        }

        public async Task DeleteUserCreditAsync(int UserCreditId, int ModuleId)
        {
            // CHANGE: Used UserCreditApiurl instead of Apiurl
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{UserCreditApiurl}/{UserCreditId}", EntityNames.Module, ModuleId));
        }

        // CreditPackage Methods
        private string CreditPackageApiurl => CreateApiUrl("CreditPackage");

        public async Task<List<CreditPackage>> GetCreditPackagesAsync(int ModuleId)
        {
            return await GetJsonAsync<List<CreditPackage>>(CreateAuthorizationPolicyUrl($"{CreditPackageApiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<CreditPackage> GetCreditPackageAsync(int CreditPackageId, int ModuleId)
        {
            return await GetJsonAsync<CreditPackage>(CreateAuthorizationPolicyUrl($"{CreditPackageApiurl}/{CreditPackageId}", EntityNames.Module, ModuleId));
        }

        public async Task<CreditPackage> AddCreditPackageAsync(CreditPackage CreditPackage)
        {
            return await PostJsonAsync<CreditPackage>(CreateAuthorizationPolicyUrl($"{CreditPackageApiurl}", EntityNames.Module, CreditPackage.ModuleId), CreditPackage);
        }

        public async Task<CreditPackage> UpdateCreditPackageAsync(CreditPackage CreditPackage)
        {
            return await PutJsonAsync<CreditPackage>(CreateAuthorizationPolicyUrl($"{CreditPackageApiurl}/{CreditPackage.CreditPackageId}", EntityNames.Module, CreditPackage.ModuleId), CreditPackage);
        }

        public async Task DeleteCreditPackageAsync(int CreditPackageId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{CreditPackageApiurl}/{CreditPackageId}", EntityNames.Module, ModuleId));
        }

        // CreditTransaction Methods
        private string CreditTransactionApiurl => CreateApiUrl("CreditTransaction");

        public async Task<List<CreditTransaction>> GetCreditTransactionsAsync(int ModuleId)
        {
            return await GetJsonAsync<List<CreditTransaction>>(CreateAuthorizationPolicyUrl($"{CreditTransactionApiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<List<CreditTransaction>> GetCreditTransactionsByUserAsync(int ModuleId, int UserId)
        {
            return await GetJsonAsync<List<CreditTransaction>>(CreateAuthorizationPolicyUrl($"{CreditTransactionApiurl}/user/{UserId}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<CreditTransaction> GetCreditTransactionAsync(int TransactionId, int ModuleId)
        {
            return await GetJsonAsync<CreditTransaction>(CreateAuthorizationPolicyUrl($"{CreditTransactionApiurl}/{TransactionId}", EntityNames.Module, ModuleId));
        }

        public async Task<CreditTransaction> AddCreditTransactionAsync(CreditTransaction CreditTransaction, int ModuleId)
        {
            return await PostJsonAsync<CreditTransaction>(CreateAuthorizationPolicyUrl($"{CreditTransactionApiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), CreditTransaction);
        }

        public async Task<CreditTransaction> UpdateCreditTransactionAsync(CreditTransaction CreditTransaction, int ModuleId)
        {
            return await PutJsonAsync<CreditTransaction>(CreateAuthorizationPolicyUrl($"{CreditTransactionApiurl}/{CreditTransaction.TransactionId}?moduleid={ModuleId}", EntityNames.Module, ModuleId), CreditTransaction);
        }

        public async Task DeleteCreditTransactionAsync(int TransactionId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{CreditTransactionApiurl}/{TransactionId}", EntityNames.Module, ModuleId));
        }

        // PaymentRecord Methods
        private string PaymentRecordApiurl => CreateApiUrl("PaymentRecord");

        public async Task<List<Models.PaymentRecord>> GetPaymentRecordsAsync(int moduleId)
        {
            return await GetJsonAsync<List<Models.PaymentRecord>>($"{PaymentRecordApiurl}?moduleid={moduleId}");
        }

        public async Task<List<Models.PaymentRecord>> GetPaymentRecordsByUserAsync(int moduleId, int userId)
        {
            return await GetJsonAsync<List<Models.PaymentRecord>>($"{PaymentRecordApiurl}/user/{userId}?moduleid={moduleId}");
        }

        public async Task<Models.PaymentRecord> GetPaymentRecordAsync(int paymentId, int moduleId)
        {
            return await GetJsonAsync<Models.PaymentRecord>($"{PaymentRecordApiurl}/{paymentId}?moduleid={moduleId}");
        }

        public async Task<Models.PaymentRecord> AddPaymentRecordAsync(Models.PaymentRecord paymentRecord)
        {
            return await PostJsonAsync<Models.PaymentRecord>($"{PaymentRecordApiurl}?moduleid={paymentRecord.ModuleId}", paymentRecord);
        }

        public async Task<Models.PaymentRecord> UpdatePaymentRecordAsync(Models.PaymentRecord paymentRecord)
        {
            return await PutJsonAsync<Models.PaymentRecord>($"{PaymentRecordApiurl}/{paymentRecord.PaymentId}?moduleid={paymentRecord.ModuleId}", paymentRecord);
        }

        public async Task DeletePaymentRecordAsync(int paymentId, int moduleId)
        {
            await DeleteAsync($"{PaymentRecordApiurl}/{paymentId}?moduleid={moduleId}");
        }


    }
}