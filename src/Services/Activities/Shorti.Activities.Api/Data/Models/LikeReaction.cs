namespace Shorti.Activities.Api.Data.Models
{
    public class LikeReaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShortId { get; set; }
    }
}
