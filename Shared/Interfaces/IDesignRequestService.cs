using GIBS.Module.DesignRequest.Models;
using Oqtane.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.DesignRequest.Services
{
    public interface IDesignRequestService
    {
        Task<List<User>> GetUsersAsync();

        Task<List<User>> GetUsersByRoleAsync(int siteId, string roleName);

        Task<List<Models.DesignRequest>> GetDesignRequestsAsync(int moduleId);
        Task<Paged<Models.DesignRequest>> GetDesignRequestsAsync(int moduleId, int page, int pageSize);
        Task<Models.DesignRequest> GetDesignRequestAsync(int DesignRequestId, int ModuleId);
        Task<Models.DesignRequest> AddDesignRequestAsync(Models.DesignRequest DesignRequest);
        Task<Models.DesignRequest> UpdateDesignRequestAsync(Models.DesignRequest DesignRequest);
        Task DeleteDesignRequestAsync(int DesignRequestId, int ModuleId);

        Task<List<Appliance>> GetAppliancesAsync(int moduleId);
        Task<Appliance> GetApplianceAsync(int applianceId, int moduleId);
        Task<Appliance> AddApplianceAsync(Appliance appliance);
        Task<Appliance> UpdateApplianceAsync(Appliance appliance);
        Task DeleteApplianceAsync(int applianceId, int moduleId);

        Task<List<Detail>> GetDetailsAsync(int moduleId);
        Task<Detail> GetDetailAsync(int detailId, int moduleId);
        Task<Detail> AddDetailAsync(Detail detail);
        Task<Detail> UpdateDetailAsync(Detail detail);
        Task DeleteDetailAsync(int detailId, int moduleId);

        Task<List<ApplianceToRequest>> GetApplianceToRequestsAsync(int designRequestId, int moduleId);
        Task<ApplianceToRequest> GetApplianceToRequestAsync(int applianceToRequestId, int moduleId);
        Task<ApplianceToRequest> AddApplianceToRequestAsync(ApplianceToRequest applianceToRequest);
        Task<ApplianceToRequest> UpdateApplianceToRequestAsync(ApplianceToRequest applianceToRequest);
        Task DeleteApplianceToRequestAsync(int applianceToRequestId, int moduleId);

        Task<List<DetailToRequest>> GetDetailToRequestsAsync(int designRequestId, int moduleId);
        Task<DetailToRequest> GetDetailToRequestAsync(int detailToRequestId, int moduleId);
        Task<DetailToRequest> AddDetailToRequestAsync(DetailToRequest detailToRequest);
        Task<DetailToRequest> UpdateDetailToRequestAsync(DetailToRequest detailToRequest);
        Task DeleteDetailToRequestAsync(int detailToRequestId, int moduleId);

        Task<List<NoteToRequest>> GetNoteToRequestsAsync(int designRequestId, int moduleId);
        Task<NoteToRequest> GetNoteToRequestAsync(int noteId, int moduleId);
        Task<NoteToRequest> AddNoteToRequestAsync(NoteToRequest noteToRequest);
        Task<NoteToRequest> UpdateNoteToRequestAsync(NoteToRequest noteToRequest);
        Task DeleteNoteToRequestAsync(int noteId, int moduleId);

        // FileToRequest methods
        Task<List<FileToRequest>> GetFileToRequestsAsync(int designRequestId, int moduleId);
        Task<FileToRequest> GetFileToRequestAsync(int fileToRequestId, int moduleId);
        Task<FileToRequest> AddFileToRequestAsync(FileToRequest fileToRequest);
        Task<FileToRequest> UpdateFileToRequestAsync(FileToRequest fileToRequest);
        Task DeleteFileToRequestAsync(int fileToRequestId, int moduleId);

        // NotificationToRequest methods
        Task<List<NotificationToRequest>> GetNotificationToRequestsAsync(int designRequestId, int moduleId);
        Task<NotificationToRequest> GetNotificationToRequestAsync(int notificationToRequestId, int moduleId);
        Task<NotificationToRequest> AddNotificationToRequestAsync(NotificationToRequest notificationToRequest);
        Task<NotificationToRequest> UpdateNotificationToRequestAsync(NotificationToRequest notificationToRequest);
        Task DeleteNotificationToRequestAsync(int notificationToRequestId, int moduleId);
    }
}