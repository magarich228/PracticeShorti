using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shorti.Shared.Contracts.Shorts
{
    public class NewShortVideoDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(200)]
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
