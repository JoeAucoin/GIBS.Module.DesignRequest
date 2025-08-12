using GIBS.Module.DesignRequest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface IDesignRequestRepository
    {
        IEnumerable<Models.DesignRequest> GetDesignRequests(int ModuleId);
        Models.DesignRequest GetDesignRequest(int DesignRequestId);
        Models.DesignRequest GetDesignRequest(int DesignRequestId, bool tracking);
        Models.DesignRequest AddDesignRequest(Models.DesignRequest DesignRequest);
        Models.DesignRequest UpdateDesignRequest(Models.DesignRequest DesignRequest);
        void DeleteDesignRequest(int DesignRequestId);

        // New methods for pagination
        IEnumerable<Models.DesignRequest> GetDesignRequests(int moduleId, int page, int pageSize);
        int CountDesignRequests(int moduleId);

        // FileToRequest methods
        IEnumerable<Models.FileToRequest> GetFileToRequests(int DesignRequestId);
        Models.FileToRequest GetFileToRequest(int FileToRequestId);
        Models.FileToRequest AddFileToRequest(Models.FileToRequest FileToRequest);
        Models.FileToRequest UpdateFileToRequest(Models.FileToRequest FileToRequest);
        void DeleteFileToRequest(int FileToRequestId);
    }
}