using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shorti.Shared.Contracts.Shorts
{
    public class NewShortVideoDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}