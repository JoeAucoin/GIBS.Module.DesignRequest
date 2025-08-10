using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class DetailRepository : IDetailRepository, ITransientService
    {
        private readonly DesignRequestContext _db;

        public DetailRepository(DesignRequestContext context)
        {
            _db = context;
        }

        public IEnumerable<Detail> GetDetails(int moduleId)
        {
            return _db.Detail.Where(item => item.ModuleId == moduleId);
        }

        public Detail GetDetail(int detailId)
        {
            return _db.Detail.Find(detailId);
        }

        public Detail AddDetail(Detail detail)
        {
            _db.Detail.Add(detail);
            _db.SaveChanges();
            return detail;
        }

        public Detail UpdateDetail(Detail detail)
        {
            _db.Entry(detail).State = EntityState.Modified;
            _db.SaveChanges();
            return detail;
        }

        public void DeleteDetail(int detailId)
        {
            Detail detail = _db.Detail.Find(detailId);
            _db.Detail.Remove(detail);
            _db.SaveChanges();
        }
    }
}