using Microsoft.AspNetCore.Http;

namespace Shorti.Shared.Kernel.Abstractions
{
    public interface IFileDownloader
    {
        Task Download(IFormFile file);
    }
}
