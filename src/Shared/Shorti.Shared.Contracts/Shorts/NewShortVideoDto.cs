using Microsoft.AspNetCore.Http;

namespace Shorti.Shared.Contracts.Shorts
{
    public class NewShortVideoDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public IFormFile File { get; set; } = null!;
    }
}
