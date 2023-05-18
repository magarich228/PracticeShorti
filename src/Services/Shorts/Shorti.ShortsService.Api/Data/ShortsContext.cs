using Microsoft.EntityFrameworkCore;
using Shorti.ShortsService.Api.Data.Models;

namespace Shorti.ShortsService.Api.Data
{
    public class ShortsContext : DbContext
    {
        public DbSet<ShortVideo> Shorts { get; set; } = null!;

        public ShortsContext()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
