using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shorti.Shared.Kernel.Abstractions;

namespace Shorti.Shared.Kernel.Services
{
    public class FileService : IFileService
    {
        private readonly IFileDownloader _fileDownloader;
        public string FilePath => _fileDownloader.FilesPath;

        public FileService(
            IFileDownloader fileDownloader,
            ILoggerFactory loggerFactory)
        {
            _fileDownloader = fileDownloader;
        }

        public async Task Download(IFormFile file) => await _fileDownloader.Download(file);
    }
}
