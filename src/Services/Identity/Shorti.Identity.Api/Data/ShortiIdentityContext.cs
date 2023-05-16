using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Shorti.Identity.Api.Data;

public class ShortiIdentityContext : IdentityDbContext<User>
{
    public ShortiIdentityContext(DbContextOptions<ShortiIdentityContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
