using Microsoft.AspNetCore.Http;

namespace Shorti.Shared.Kernel.Abstractions
{
    public interface IFileDownloader
    {
        string FilesPath { get; }

        Task Download(IFormFile file, string fileName);
    }
}
