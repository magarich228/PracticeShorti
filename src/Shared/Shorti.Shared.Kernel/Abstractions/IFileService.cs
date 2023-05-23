using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shorti.Shared.Kernel.Abstractions
{
    public interface IFileService
    {
        string FilePath { get; }
        Task DownloadAsync(IFormFile file, string fileName);
        Task<IEnumerable<ValidationResult>> ValidateFile(IFormFile file);
    }
}
