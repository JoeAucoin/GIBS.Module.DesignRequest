using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class ApplianceRepository : IApplianceRepository, ITransientService
    {
        private readonly DesignRequestContext _db;

        public ApplianceRepository(DesignRequestContext context)
        {
            _db = context;
        }

        public IEnumerable<Appliance> GetAppliances(int moduleId)
        {
            return _db.Appliance.Where(item => item.ModuleId == moduleId);
        }

        public Appliance GetAppliance(int applianceId)
        {
            return _db.Appliance.Find(applianceId);
        }

        public Appliance AddAppliance(Appliance appliance)
        {
            _db.Appliance.Add(appliance);
            _db.SaveChanges();
            return appliance;
        }

        public Appliance UpdateAppliance(Appliance appliance)
        {
            _db.Entry(appliance).State = EntityState.Modified;
            _db.SaveChanges();
            return appliance;
        }

        public void DeleteAppliance(int applianceId)
        {
            Appliance appliance = _db.Appliance.Find(applianceId);
            _db.Appliance.Remove(appliance);
            _db.SaveChanges();
        }
    }
}