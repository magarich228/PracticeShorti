using Microsoft.EntityFrameworkCore;
using Shorti.Activities.Api.Data.Models;

namespace Shorti.Activities.Api.Data
{
    public class ActivitiesContext : DbContext
    {
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<LikeReaction> Likes { get; set; } = null!;

        public ActivitiesContext(DbContextOptions<ActivitiesContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
