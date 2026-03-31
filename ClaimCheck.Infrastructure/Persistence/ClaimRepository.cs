using ClaimCheck.Application.Claims;
using ClaimCheck.Domain.Claims;

namespace ClaimCheck.Infrastructure.Persistence;

public sealed class ClaimRepository(AppDbContext db) : IClaimRepository
{
    public async Task SaveAsync(ClaimAnalysis analysis, CancellationToken ct = default)
    {
        db.ClaimAnalyses.Add(analysis);
        await db.SaveChangesAsync(ct);
    }
}
