using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using user.domain;

namespace user.infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public DbSet<IdentityUserClaim<string>> AspNetUserClaims { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("identity");
    }
}
