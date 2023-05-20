using Microsoft.EntityFrameworkCore;

namespace Shorti.Identity.Api.Data;

public class ShortiIdentityContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public ShortiIdentityContext(DbContextOptions<ShortiIdentityContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasAlternateKey(u => u.UserName);

        base.OnModelCreating(builder);
    }
}
