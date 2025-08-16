using System.Collections.Generic;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface INotificationToRequestRepository
    {
        IEnumerable<NotificationToRequest> GetNotificationToRequests(int designRequestId);
        NotificationToRequest GetNotificationToRequest(int notificationToRequestId, bool tracking);
        NotificationToRequest AddNotificationToRequest(NotificationToRequest notificationToRequest);
        NotificationToRequest UpdateNotificationToRequest(NotificationToRequest notificationToRequest);
        void DeleteNotificationToRequest(int notificationToRequestId);
    }
}