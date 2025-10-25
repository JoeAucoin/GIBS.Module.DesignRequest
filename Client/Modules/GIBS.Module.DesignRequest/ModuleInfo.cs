using Oqtane.Models;
using Oqtane.Modules;

namespace GIBS.Module.DesignRequest
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "DesignRequest",
            Description = "Design Request Module for Oqtane",
            Version = "1.0.4",
            ServerManagerType = "GIBS.Module.DesignRequest.Manager.DesignRequestManager, GIBS.Module.DesignRequest.Server.Oqtane",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2,1.0.3,1.0.4",
            Dependencies = "GIBS.Module.DesignRequest.Shared.Oqtane",
            PackageName = "GIBS.Module.DesignRequest" 
        };
    }
}
