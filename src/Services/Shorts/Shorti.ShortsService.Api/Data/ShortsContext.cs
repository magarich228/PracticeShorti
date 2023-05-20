using Microsoft.EntityFrameworkCore;
using Shorti.ShortsService.Api.Data.Models;

namespace Shorti.ShortsService.Api.Data
{
    public class ShortsContext : DbContext
    {
        public DbSet<ShortVideo> Shorts { get; set; } = null!;

        public ShortsContext(DbContextOptions<ShortsContext> options)
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
