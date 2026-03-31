using ClaimCheck.Domain.Claims;
using Microsoft.EntityFrameworkCore;

namespace ClaimCheck.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ClaimAnalysis> ClaimAnalyses => Set<ClaimAnalysis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClaimAnalysis>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ClaimText).IsRequired();
            e.OwnsOne(x => x.Result, r => r.ToJson());
        });
    }
}
