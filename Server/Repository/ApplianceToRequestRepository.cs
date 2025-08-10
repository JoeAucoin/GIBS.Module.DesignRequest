using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class ApplianceToRequestRepository : IApplianceToRequestRepository, ITransientService
    {
        private readonly DesignRequestContext _db;

        public ApplianceToRequestRepository(DesignRequestContext context)
        {
            _db = context;
        }

        public IEnumerable<ApplianceToRequest> GetApplianceToRequests(int designRequestId)
        {
            return _db.ApplianceToRequest.Include(i => i.Appliance).Where(x => x.DesignRequestId == designRequestId).AsNoTracking();
        }

        public ApplianceToRequest GetApplianceToRequest(int applianceToRequestId)
        {
            return _db.ApplianceToRequest.AsNoTracking().FirstOrDefault(item => item.ApplianceToRequestId == applianceToRequestId);
        }

        public ApplianceToRequest AddApplianceToRequest(ApplianceToRequest applianceToRequest)
        {
            _db.ApplianceToRequest.Add(applianceToRequest);
            _db.SaveChanges();
            return applianceToRequest;
        }

        public ApplianceToRequest UpdateApplianceToRequest(ApplianceToRequest applianceToRequest)
        {
            //_db.Entry(applianceToRequest).State = EntityState.Modified;
            _db.Update(applianceToRequest);
            _db.Entry(applianceToRequest).Property(x => x.CreatedBy).IsModified = false;
            _db.Entry(applianceToRequest).Property(x => x.CreatedOn).IsModified = false;
            _db.SaveChanges();
            return applianceToRequest;
        }

        public void DeleteApplianceToRequest(int applianceToRequestId)
        {
            ApplianceToRequest applianceToRequest = _db.ApplianceToRequest.Find(applianceToRequestId);
            _db.ApplianceToRequest.Remove(applianceToRequest);
            _db.SaveChanges();
        }
    }
}