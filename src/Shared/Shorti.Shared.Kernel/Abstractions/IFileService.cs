using Microsoft.AspNetCore.Http;

namespace Shorti.Shared.Kernel.Abstractions
{
    public interface IFileService
    {
        string FilePath { get; }

        Task Download(IFormFile file);
    }
}
