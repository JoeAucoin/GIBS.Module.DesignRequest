using System.Collections.Generic;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface IApplianceRepository
    {
        IEnumerable<Appliance> GetAppliances(int moduleId);
        Appliance GetAppliance(int applianceId);
        Appliance AddAppliance(Appliance appliance);
        Appliance UpdateAppliance(Appliance appliance);
        void DeleteAppliance(int applianceId);
    }
}