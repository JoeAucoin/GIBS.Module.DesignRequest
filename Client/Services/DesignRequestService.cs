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
            return await GetJsonAsync<List<User>>($"{Apiurl}/users");
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
            return await GetJsonAsync<List<ApplianceToRequest>>($"{ApplianceToRequestApiurl}?designrequestid={designRequestId}&moduleid={moduleId}");
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
            return await GetJsonAsync<List<DetailToRequest>>($"{DetailToRequestApiurl}?designrequestid={designRequestId}&moduleid={moduleId}");
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
            return await GetJsonAsync<List<NoteToRequest>>($"{NoteToRequestApiurl}?designrequestid={designRequestId}&moduleid={moduleId}");
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
            return await GetJsonAsync<List<FileToRequest>>($"{FileToRequestApiurl}?designrequestid={designRequestId}&moduleid={moduleId}");
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
    }
}