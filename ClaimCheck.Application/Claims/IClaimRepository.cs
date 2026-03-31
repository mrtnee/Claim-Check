using ClaimCheck.Domain.Claims;

namespace ClaimCheck.Application.Claims;

public interface IClaimRepository
{
    Task SaveAsync(ClaimAnalysis analysis, CancellationToken ct = default);
}
