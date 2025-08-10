using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class DetailToRequestRepository : IDetailToRequestRepository, ITransientService
    {
        private readonly DesignRequestContext _db;

        public DetailToRequestRepository(DesignRequestContext context)
        {
            _db = context;
        }

        public IEnumerable<DetailToRequest> GetDetailToRequests(int designRequestId)
        {
            return _db.DetailToRequest.Include(i => i.Detail).Where(x => x.DesignRequestId == designRequestId).AsNoTracking();
        }

        public DetailToRequest GetDetailToRequest(int detailToRequestId)
        {
            return _db.DetailToRequest.AsNoTracking().FirstOrDefault(item => item.DetailToRequestId == detailToRequestId);
        }

        public DetailToRequest AddDetailToRequest(DetailToRequest detailToRequest)
        {
            _db.DetailToRequest.Add(detailToRequest);
            _db.SaveChanges();
            return detailToRequest;
        }

        public DetailToRequest UpdateDetailToRequest(DetailToRequest detailToRequest)
        {
            _db.Update(detailToRequest);
            _db.Entry(detailToRequest).Property(x => x.CreatedBy).IsModified = false;
            _db.Entry(detailToRequest).Property(x => x.CreatedOn).IsModified = false;
            _db.SaveChanges();
            return detailToRequest;
        }

        public void DeleteDetailToRequest(int detailToRequestId)
        {
            DetailToRequest detailToRequest = _db.DetailToRequest.Find(detailToRequestId);
            _db.DetailToRequest.Remove(detailToRequest);
            _db.SaveChanges();
        }
    }
}