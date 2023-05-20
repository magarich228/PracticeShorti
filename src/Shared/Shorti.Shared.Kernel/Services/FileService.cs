using Microsoft.AspNetCore.Http;
using Shorti.Shared.Kernel.Abstractions;

namespace Shorti.Shared.Kernel.Services
{
    public class FileService : IFileService
    {
        private readonly IFileDownloader _fileDownloader;
        public string FilePath => _fileDownloader.FilesPath;

        public FileService(IFileDownloader fileDownloader)
        {
            _fileDownloader = fileDownloader;
        }

        public async Task DownloadAsync(IFormFile file, string fileName) => await _fileDownloader.Download(file, fileName);
    }
}
