using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shorti.Shared.Kernel.Abstractions
{
    public interface IFileValidator
    {
        IEnumerable<ValidationResult> Validate(IFormFile file); 
    }
}
