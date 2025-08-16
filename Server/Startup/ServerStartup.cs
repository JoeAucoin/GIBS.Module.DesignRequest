using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using GIBS.Module.DesignRequest.Repository;
using GIBS.Module.DesignRequest.Services;

namespace GIBS.Module.DesignRequest.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDesignRequestService, ServerDesignRequestService>();

            services.AddTransient<IDesignRequestRepository, DesignRequestRepository>();
            services.AddTransient<IApplianceRepository, ApplianceRepository>();
            services.AddTransient<IDetailRepository, DetailRepository>();
            services.AddTransient<IApplianceToRequestRepository, ApplianceToRequestRepository>();
            services.AddTransient<IDetailToRequestRepository, DetailToRequestRepository>();
            services.AddTransient<INoteToRequestRepository, NoteToRequestRepository>();
            services.AddTransient<IFileToRequestRepository, FileToRequestRepository>();
            services.AddTransient<INotificationToRequestRepository, NotificationToRequestRepository>();

            services.AddDbContextFactory<DesignRequestContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}