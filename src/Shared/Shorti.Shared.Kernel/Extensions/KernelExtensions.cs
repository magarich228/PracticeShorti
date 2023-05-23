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
            FileDownloaderSettings fileDownloaderSettings = new();
            FileValidatorSettings fileValidatorSettings = new ();

            configuration.Bind(fileDownloaderSettings);
            configuration.Bind(fileValidatorSettings);

            services.AddTransient<IFileDownloader, FileDownloader>(services => 
                new FileDownloader(fileDownloaderSettings));

            services.AddTransient<IFileValidator, FileValidator>(services => 
                new FileValidator(fileValidatorSettings));

            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
