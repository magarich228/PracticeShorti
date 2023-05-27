using Microsoft.Extensions.DependencyInjection;

namespace Shorti.Shared.Contracts.Services
{
    public static class ServicesContractsExtensions
    {
        public static IServiceCollection AddContractsServicesClients(this IServiceCollection services)
        {
            services.AddHttpClient("ApiGwHost", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5171/");
            });

            services.AddHttpClient("ShortsHost", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5059/");
            });

            services.AddHttpClient("IdentityHost", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5064/");
            });

            services.AddHttpClient("ActivitiesHost", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5031/");
            });

            services.AddTransient<IIdentityServiceClient, IdentityServiceClient>();
            services.AddTransient<IShortsServiceClient, ShortsServiceClient>();

            return services;
        }
    }
}
