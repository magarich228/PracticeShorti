using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shorti.Shared.Kernel.Abstractions;

namespace Shorti.Shared.Kernel
{
    public class FileDownloader : IFileDownloader
    {
        private string _filesPath;
        private string _fileDownloaderSettingsKey = "FileDownloader";

        public string FilesPath => _filesPath;

        public FileDownloader(string filesPath)
        {
            _filesPath = filesPath;
        }

        public FileDownloader(FileDownloaderSettings settings)
        {
            _filesPath = settings.FilesPath;
        }

        public async Task Download(IFormFile file, string fileName)
        {
            ArgumentNullException.ThrowIfNull(file, nameof(file));

            using var newFile = File.Create(Path.Combine(_filesPath, fileName));
            using var read = file.OpenReadStream();

            await read.CopyToAsync(newFile);

            await newFile.FlushAsync();
        }
    }
}
