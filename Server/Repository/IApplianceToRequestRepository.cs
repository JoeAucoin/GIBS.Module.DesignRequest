using System.Collections.Generic;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface IApplianceToRequestRepository
    {
        IEnumerable<ApplianceToRequest> GetApplianceToRequests(int designRequestId);
        ApplianceToRequest GetApplianceToRequest(int applianceToRequestId);
        ApplianceToRequest AddApplianceToRequest(ApplianceToRequest applianceToRequest);
        ApplianceToRequest UpdateApplianceToRequest(ApplianceToRequest applianceToRequest);
        void DeleteApplianceToRequest(int applianceToRequestId);
    }
}