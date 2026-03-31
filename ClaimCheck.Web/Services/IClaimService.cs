using ClaimCheck.Web.Models;

namespace ClaimCheck.Web.Services;

public interface IClaimService
{
    Task<ClaimResultModel> AnalyzeAsync(string claimText, CancellationToken ct = default);
}
