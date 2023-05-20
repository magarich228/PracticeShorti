using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shorti.Shared.Kernel.Abstractions;
using Shorti.Shared.Kernel.Services;

namespace Shorti.Shared.Kernel.KernelExtensions
{
    public static class KernelExtensions
    {
        public static IServiceCollection AddKernelServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IFileDownloader, FileDownloader>(services => 
                new FileDownloader(configuration));

            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
