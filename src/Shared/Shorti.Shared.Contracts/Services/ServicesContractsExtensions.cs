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

            services.AddTransient<IIdentityServiceClient, IdentityServiceClient>();
            services.AddTransient<IShortsServiceClient, ShortsServiceClient>();

            return services;
        }
    }
}
