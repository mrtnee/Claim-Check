using ClaimCheck.Domain.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClaimCheck.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
  public DbSet<ClaimAnalysis> ClaimAnalyses => Set<ClaimAnalysis>();

  // As the number of entities grows, consider moving each entity's configuration
  // into its own IEntityTypeConfiguration<T> class and replacing the body here with:
  // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder); // required for Identity tables

    modelBuilder.Entity<ClaimAnalysis>(e =>
    {
      e.HasKey(x => x.Id);
      e.Property(x => x.ClaimText).IsRequired();
      e.Property(x => x.UserId).IsRequired();
      e.OwnsOne(x => x.Result, r => r.ToJson());
    });
  }
}
