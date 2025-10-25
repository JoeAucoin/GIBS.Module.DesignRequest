using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.DesignRequest.Repository
{
    public class DesignRequestRepository : IDesignRequestRepository, ITransientService
    {
        private readonly IDbContextFactory<DesignRequestContext> _factory;

        public DesignRequestRepository(IDbContextFactory<DesignRequestContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.DesignRequest> GetDesignRequests(int ModuleId)
        {
            //using var db = _factory.CreateDbContext();
            //return db.DesignRequest.Where(item => item.ModuleId == ModuleId).ToList();
            using var db = _factory.CreateDbContext();
            return db.DesignRequest.Include(item => item.Files).Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.DesignRequest GetDesignRequest(int DesignRequestId)
        {
            return GetDesignRequest(DesignRequestId, true);
        }

        public Models.DesignRequest GetDesignRequest(int DesignRequestId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.DesignRequest.Find(DesignRequestId);
            }
            else
            {
                return db.DesignRequest.AsNoTracking().FirstOrDefault(item => item.DesignRequestId == DesignRequestId);
            }
        }

        public Models.DesignRequest AddDesignRequest(Models.DesignRequest DesignRequest)
        {
            using var db = _factory.CreateDbContext();
            db.DesignRequest.Add(DesignRequest);
            db.SaveChanges();
            return DesignRequest;
        }

        public Models.DesignRequest UpdateDesignRequest(Models.DesignRequest DesignRequest)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(DesignRequest).State = EntityState.Modified;
            db.SaveChanges();
            return DesignRequest;
        }

        public void DeleteDesignRequest(int DesignRequestId)
        {
            using var db = _factory.CreateDbContext();
            Models.DesignRequest DesignRequest = db.DesignRequest.Find(DesignRequestId);
            db.DesignRequest.Remove(DesignRequest);
            db.SaveChanges();
        }

        // New method implementations for pagination
        public IEnumerable<Models.DesignRequest> GetDesignRequests(int moduleId, int page, int pageSize)
        {
            using var db = _factory.CreateDbContext();
            return db.DesignRequest
                .Where(r => r.ModuleId == moduleId)
                .OrderByDescending(r => r.DesignRequestId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int CountDesignRequests(int moduleId)
        {
            using var db = _factory.CreateDbContext();
            return db.DesignRequest
                .Count(r => r.ModuleId == moduleId);
        }


        // FileToRequest methods
        public IEnumerable<Models.FileToRequest> GetFileToRequests(int DesignRequestId)
        {
            //using var db = _factory.CreateDbContext();
            //return db.FileToRequest.Where(item => item.DesignRequestId == DesignRequestId).ToList();
            using var db = _factory.CreateDbContext();
            return db.FileToRequest.Where(item => item.DesignRequestId == DesignRequestId).OrderByDescending(f => f.CreatedOn).ToList();
        }

        public Models.FileToRequest GetFileToRequest(int FileToRequestId)
        {
            using var db = _factory.CreateDbContext();
            return db.FileToRequest.Find(FileToRequestId);
        }

        public Models.FileToRequest AddFileToRequest(Models.FileToRequest FileToRequest)
        {
            using var db = _factory.CreateDbContext();
            db.FileToRequest.Add(FileToRequest);
            db.SaveChanges();
            return FileToRequest;
        }

        public Models.FileToRequest UpdateFileToRequest(Models.FileToRequest FileToRequest)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(FileToRequest).State = EntityState.Modified;
            db.SaveChanges();
            return FileToRequest;
        }

        public void DeleteFileToRequest(int FileToRequestId)
        {
            using var db = _factory.CreateDbContext();
            Models.FileToRequest FileToRequest = db.FileToRequest.Find(FileToRequestId);
            db.FileToRequest.Remove(FileToRequest);
            db.SaveChanges();
        }
    }
}