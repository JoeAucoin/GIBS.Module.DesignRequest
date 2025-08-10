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

        public async Task<List<Models.DesignRequest>> GetDesignRequestsAsync(int ModuleId)
        {
            List<Models.DesignRequest> DesignRequests = await GetJsonAsync<List<Models.DesignRequest>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.DesignRequest>().ToList());
            return DesignRequests.OrderBy(item => item.DesignRequestId).ToList();
        }

        public async Task<Models.DesignRequest> GetDesignRequestAsync(int DesignRequestId, int ModuleId)
        {
            return await GetJsonAsync<Models.DesignRequest>(CreateAuthorizationPolicyUrl($"{Apiurl}/{DesignRequestId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.DesignRequest> AddDesignRequestAsync(Models.DesignRequest DesignRequest)
        {
            return await PostJsonAsync<Models.DesignRequest>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, DesignRequest.ModuleId), DesignRequest);
        }

        public async Task<Models.DesignRequest> UpdateDesignRequestAsync(Models.DesignRequest DesignRequest)
        {
            return await PutJsonAsync<Models.DesignRequest>(CreateAuthorizationPolicyUrl($"{Apiurl}/{DesignRequest.DesignRequestId}", EntityNames.Module, DesignRequest.ModuleId), DesignRequest);
        }

        public async Task DeleteDesignRequestAsync(int DesignRequestId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{DesignRequestId}", EntityNames.Module, ModuleId));
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await GetJsonAsync<List<User>>($"{Apiurl}/users");
        }

        // Appliance Methods
        private string ApplianceApiurl => CreateApiUrl("Appliance");

        public async Task<List<Appliance>> GetAppliancesAsync(int moduleId)
        {
            return await GetJsonAsync<List<Appliance>>(CreateAuthorizationPolicyUrl($"{ApplianceApiurl}?moduleid={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<Appliance> GetApplianceAsync(int applianceId, int moduleId)
        {
            return await GetJsonAsync<Appliance>(CreateAuthorizationPolicyUrl($"{ApplianceApiurl}/{applianceId}", EntityNames.Module, moduleId));
        }

        public async Task<Appliance> AddApplianceAsync(Appliance appliance)
        {
            return await PostJsonAsync<Appliance>(CreateAuthorizationPolicyUrl($"{ApplianceApiurl}", EntityNames.Module, appliance.ModuleId), appliance);
        }

        public async Task<Appliance> UpdateApplianceAsync(Appliance appliance)
        {
            return await PutJsonAsync<Appliance>(CreateAuthorizationPolicyUrl($"{ApplianceApiurl}/{appliance.ApplianceId}", EntityNames.Module, appliance.ModuleId), appliance);
        }

        public async Task DeleteApplianceAsync(int applianceId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{ApplianceApiurl}/{applianceId}", EntityNames.Module, moduleId));
        }

        // Detail Methods
        private string DetailApiurl => CreateApiUrl("Detail");

        public async Task<List<Detail>> GetDetailsAsync(int moduleId)
        {
            return await GetJsonAsync<List<Detail>>(CreateAuthorizationPolicyUrl($"{DetailApiurl}?moduleid={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<Detail> GetDetailAsync(int detailId, int moduleId)
        {
            return await GetJsonAsync<Detail>(CreateAuthorizationPolicyUrl($"{DetailApiurl}/{detailId}", EntityNames.Module, moduleId));
        }

        public async Task<Detail> AddDetailAsync(Detail detail)
        {
            return await PostJsonAsync<Detail>(CreateAuthorizationPolicyUrl($"{DetailApiurl}", EntityNames.Module, detail.ModuleId), detail);
        }

        public async Task<Detail> UpdateDetailAsync(Detail detail)
        {
            return await PutJsonAsync<Detail>(CreateAuthorizationPolicyUrl($"{DetailApiurl}/{detail.DetailId}", EntityNames.Module, detail.ModuleId), detail);
        }

        public async Task DeleteDetailAsync(int detailId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{DetailApiurl}/{detailId}", EntityNames.Module, moduleId));
        }

        // ApplianceToRequest Methods
        private string ApplianceToRequestApiurl => CreateApiUrl("ApplianceToRequest");

        public async Task<List<ApplianceToRequest>> GetApplianceToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<ApplianceToRequest>>(CreateAuthorizationPolicyUrl($"{ApplianceToRequestApiurl}?designrequestid={designRequestId}", EntityNames.Module, moduleId));
        }

        public async Task<ApplianceToRequest> GetApplianceToRequestAsync(int applianceToRequestId, int moduleId)
        {
            return await GetJsonAsync<ApplianceToRequest>(CreateAuthorizationPolicyUrl($"{ApplianceToRequestApiurl}/{applianceToRequestId}", EntityNames.Module, moduleId));
        }

        public async Task<ApplianceToRequest> AddApplianceToRequestAsync(ApplianceToRequest applianceToRequest)
        {
            var designRequest = await GetDesignRequestAsync(applianceToRequest.DesignRequestId, -1);
            return await PostJsonAsync<ApplianceToRequest>(CreateAuthorizationPolicyUrl($"{ApplianceToRequestApiurl}", EntityNames.Module, designRequest.ModuleId), applianceToRequest);
        }

        public async Task<ApplianceToRequest> UpdateApplianceToRequestAsync(ApplianceToRequest applianceToRequest)
        {
            var designRequest = await GetDesignRequestAsync(applianceToRequest.DesignRequestId, -1);
            return await PutJsonAsync<ApplianceToRequest>(CreateAuthorizationPolicyUrl($"{ApplianceToRequestApiurl}/{applianceToRequest.ApplianceToRequestId}", EntityNames.Module, designRequest.ModuleId), applianceToRequest);
        }

        public async Task DeleteApplianceToRequestAsync(int applianceToRequestId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{ApplianceToRequestApiurl}/{applianceToRequestId}", EntityNames.Module, moduleId));
        }

        // DetailToRequest Methods
        private string DetailToRequestApiurl => CreateApiUrl("DetailToRequest");

        public async Task<List<DetailToRequest>> GetDetailToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<DetailToRequest>>(CreateAuthorizationPolicyUrl($"{DetailToRequestApiurl}?designrequestid={designRequestId}", EntityNames.Module, moduleId));
        }

        public async Task<DetailToRequest> GetDetailToRequestAsync(int detailToRequestId, int moduleId)
        {
            return await GetJsonAsync<DetailToRequest>(CreateAuthorizationPolicyUrl($"{DetailToRequestApiurl}/{detailToRequestId}", EntityNames.Module, moduleId));
        }

        public async Task<DetailToRequest> AddDetailToRequestAsync(DetailToRequest detailToRequest)
        {
            var designRequest = await GetDesignRequestAsync(detailToRequest.DesignRequestId, -1);
            return await PostJsonAsync<DetailToRequest>(CreateAuthorizationPolicyUrl($"{DetailToRequestApiurl}", EntityNames.Module, designRequest.ModuleId), detailToRequest);
        }

        public async Task<DetailToRequest> UpdateDetailToRequestAsync(DetailToRequest detailToRequest)
        {
            var designRequest = await GetDesignRequestAsync(detailToRequest.DesignRequestId, -1);
            return await PutJsonAsync<DetailToRequest>(CreateAuthorizationPolicyUrl($"{DetailToRequestApiurl}/{detailToRequest.DetailToRequestId}", EntityNames.Module, designRequest.ModuleId), detailToRequest);
        }

        public async Task DeleteDetailToRequestAsync(int detailToRequestId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{DetailToRequestApiurl}/{detailToRequestId}", EntityNames.Module, moduleId));
        }

        // NoteToRequest Methods
        private string NoteToRequestApiurl => CreateApiUrl("NoteToRequest");

        public async Task<List<NoteToRequest>> GetNoteToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<NoteToRequest>>(CreateAuthorizationPolicyUrl($"{NoteToRequestApiurl}?designrequestid={designRequestId}", EntityNames.Module, moduleId));
        }

        public async Task<NoteToRequest> GetNoteToRequestAsync(int noteId, int moduleId)
        {
            return await GetJsonAsync<NoteToRequest>(CreateAuthorizationPolicyUrl($"{NoteToRequestApiurl}/{noteId}", EntityNames.Module, moduleId));
        }

        public async Task<NoteToRequest> AddNoteToRequestAsync(NoteToRequest noteToRequest)
        {
            var designRequest = await GetDesignRequestAsync(noteToRequest.DesignRequestId, -1);
            return await PostJsonAsync<NoteToRequest>(CreateAuthorizationPolicyUrl($"{NoteToRequestApiurl}", EntityNames.Module, designRequest.ModuleId), noteToRequest);
        }

        public async Task<NoteToRequest> UpdateNoteToRequestAsync(NoteToRequest noteToRequest)
        {
            var designRequest = await GetDesignRequestAsync(noteToRequest.DesignRequestId, -1);
            return await PutJsonAsync<NoteToRequest>(CreateAuthorizationPolicyUrl($"{NoteToRequestApiurl}/{noteToRequest.NoteId}", EntityNames.Module, designRequest.ModuleId), noteToRequest);
        }

        public async Task DeleteNoteToRequestAsync(int noteId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{NoteToRequestApiurl}/{noteId}", EntityNames.Module, moduleId));
        }

        // FileToRequest Methods
        private string FileToRequestApiurl => CreateApiUrl("FileToRequest");

        public async Task<List<FileToRequest>> GetFileToRequestsAsync(int designRequestId, int moduleId)
        {
            return await GetJsonAsync<List<FileToRequest>>(CreateAuthorizationPolicyUrl($"{FileToRequestApiurl}?designrequestid={designRequestId}", EntityNames.Module, moduleId));
        }

        public async Task<FileToRequest> GetFileToRequestAsync(int fileToRequestId, int moduleId)
        {
            return await GetJsonAsync<FileToRequest>(CreateAuthorizationPolicyUrl($"{FileToRequestApiurl}/{fileToRequestId}", EntityNames.Module, moduleId));
        }

        public async Task<FileToRequest> AddFileToRequestAsync(FileToRequest fileToRequest)
        {
            // The controller allows anonymous posts, so no authorization policy URL is strictly needed,
            // but we can still pass the module ID for consistency if needed in the future.
            var designRequest = await GetDesignRequestAsync(fileToRequest.DesignRequestId, -1);
            return await PostJsonAsync<FileToRequest>(CreateAuthorizationPolicyUrl(FileToRequestApiurl, EntityNames.Module, designRequest.ModuleId), fileToRequest);
        }

        public async Task<FileToRequest> UpdateFileToRequestAsync(FileToRequest fileToRequest)
        {
            var designRequest = await GetDesignRequestAsync(fileToRequest.DesignRequestId, -1);
            return await PutJsonAsync<FileToRequest>(CreateAuthorizationPolicyUrl($"{FileToRequestApiurl}/{fileToRequest.FileToRequestId}", EntityNames.Module, designRequest.ModuleId), fileToRequest);
        }

        public async Task DeleteFileToRequestAsync(int fileToRequestId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{FileToRequestApiurl}/{fileToRequestId}", EntityNames.Module, moduleId));
        }
    }
}