using System.Collections.Generic;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface IDetailToRequestRepository
    {
        IEnumerable<DetailToRequest> GetDetailToRequests(int designRequestId);
        DetailToRequest GetDetailToRequest(int detailToRequestId);
        DetailToRequest AddDetailToRequest(DetailToRequest detailToRequest);
        DetailToRequest UpdateDetailToRequest(DetailToRequest detailToRequest);
        void DeleteDetailToRequest(int detailToRequestId);
    }
}