namespace Shorti.Shared.Contracts.Activities
{
    public class LikeReactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShortId { get; set; }
    }
}
