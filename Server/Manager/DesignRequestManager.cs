using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using GIBS.Module.DesignRequest.Repository;
using System.Threading.Tasks;

namespace GIBS.Module.DesignRequest.Manager
{
    public class DesignRequestManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IDesignRequestRepository _DesignRequestRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public DesignRequestManager(IDesignRequestRepository DesignRequestRepository, IDBContextDependencies DBContextDependencies)
        {
            _DesignRequestRepository = DesignRequestRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new DesignRequestContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new DesignRequestContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.DesignRequest> DesignRequests = _DesignRequestRepository.GetDesignRequests(module.ModuleId).ToList();
            if (DesignRequests != null)
            {
                content = JsonSerializer.Serialize(DesignRequests);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.DesignRequest> DesignRequests = null;
            if (!string.IsNullOrEmpty(content))
            {
                DesignRequests = JsonSerializer.Deserialize<List<Models.DesignRequest>>(content);
            }
            if (DesignRequests != null)
            {
                foreach(var DesignRequest in DesignRequests)
                {
                    _DesignRequestRepository.AddDesignRequest(new Models.DesignRequest { ModuleId = module.ModuleId, ContactName = DesignRequest.ContactName });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var DesignRequest in _DesignRequestRepository.GetDesignRequests(pageModule.ModuleId))
           {
               if (DesignRequest.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "GIBSDesignRequest",
                       EntityId = DesignRequest.DesignRequestId.ToString(),
                       Title = DesignRequest.ContactName,
                       Body = DesignRequest.ContactName,
                       ContentModifiedBy = DesignRequest.ModifiedBy,
                       ContentModifiedOn = DesignRequest.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
