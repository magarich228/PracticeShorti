namespace Shorti.Shared.Contracts.Shorts
{
    public class ShortVideoDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public string FileName { get; set; } = null!;
    }
}
