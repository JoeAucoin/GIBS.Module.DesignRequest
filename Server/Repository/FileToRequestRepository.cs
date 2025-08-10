using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Modules;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class FileToRequestRepository : IFileToRequestRepository, ITransientService
    {
        private readonly IDbContextFactory<DesignRequestContext> _factory;

        public FileToRequestRepository(IDbContextFactory<DesignRequestContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<FileToRequest> GetFileToRequests(int designRequestId)
        {
            using var db = _factory.CreateDbContext();
            return db.FileToRequest.Where(item => item.DesignRequestId == designRequestId).ToList();
        }

        public FileToRequest GetFileToRequest(int fileToRequestId)
        {
            using var db = _factory.CreateDbContext();
            return db.FileToRequest.Find(fileToRequestId);
        }

        public FileToRequest AddFileToRequest(FileToRequest fileToRequest)
        {
            using var db = _factory.CreateDbContext();
            db.FileToRequest.Add(fileToRequest);
            db.SaveChanges();
            return fileToRequest;
        }

        public FileToRequest UpdateFileToRequest(FileToRequest fileToRequest)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(fileToRequest).State = EntityState.Modified;
            db.SaveChanges();
            return fileToRequest;
        }

        public void DeleteFileToRequest(int fileToRequestId)
        {
            using var db = _factory.CreateDbContext();
            var fileToRequest = db.FileToRequest.Find(fileToRequestId);
            if (fileToRequest != null)
            {
                db.FileToRequest.Remove(fileToRequest);
                db.SaveChanges();
            }
        }
    }
}