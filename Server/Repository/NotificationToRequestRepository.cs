using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class NotificationToRequestRepository : INotificationToRequestRepository
    {
        private readonly DesignRequestContext _db;

        public NotificationToRequestRepository(DesignRequestContext context)
        {
            _db = context;
        }

        public IEnumerable<NotificationToRequest> GetNotificationToRequests(int designRequestId)
        {
            return _db.NotificationToRequest.Where(item => item.DesignRequestId == designRequestId);
        }

        public NotificationToRequest GetNotificationToRequest(int notificationToRequestId, bool tracking)
        {
            if (tracking)
            {
                return _db.NotificationToRequest.Find(notificationToRequestId);
            }
            else
            {
                return _db.NotificationToRequest.AsNoTracking().FirstOrDefault(item => item.NotificationToRequestId == notificationToRequestId);
            }
        }

        public NotificationToRequest AddNotificationToRequest(NotificationToRequest notificationToRequest)
        {
            _db.NotificationToRequest.Add(notificationToRequest);
            _db.SaveChanges();
            return notificationToRequest;
        }

        public NotificationToRequest UpdateNotificationToRequest(NotificationToRequest notificationToRequest)
        {
            _db.Entry(notificationToRequest).State = EntityState.Modified;
            _db.SaveChanges();
            return notificationToRequest;
        }

        public void DeleteNotificationToRequest(int notificationToRequestId)
        {
            var notificationToRequest = _db.NotificationToRequest.Find(notificationToRequestId);
            if (notificationToRequest != null)
            {
                _db.NotificationToRequest.Remove(notificationToRequest);
                _db.SaveChanges();
            }
        }
    }
}