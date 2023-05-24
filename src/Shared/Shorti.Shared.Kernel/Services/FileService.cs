using Microsoft.AspNetCore.Http;
using Shorti.Shared.Kernel.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Shorti.Shared.Kernel.Services
{
    public class FileService : IFileService
    {
        private readonly IFileDownloader _fileDownloader;
        private readonly IFileValidator _fileValidator;

        public string FilePath => _fileDownloader.FilesPath;

        public FileService(
            IFileDownloader fileDownloader,
            IFileValidator fileValidator)
        {
            _fileDownloader = fileDownloader;
            _fileValidator = fileValidator;
        }

        public async Task DownloadAsync(IFormFile file, string fileName) => await _fileDownloader.Download(file, fileName);

        public async Task<IEnumerable<ValidationResult>> ValidateFile(IFormFile file) => 
            await Task.Run(() => _fileValidator.Validate(file));
    }
}
