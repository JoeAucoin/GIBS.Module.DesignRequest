using Microsoft.Extensions.DependencyInjection;
using Oqtane.Services;
using GIBS.Module.DesignRequest.Services;

namespace GIBS.Module.DesignRequest.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDesignRequestService, DesignRequestService>();
        }
    }
}
