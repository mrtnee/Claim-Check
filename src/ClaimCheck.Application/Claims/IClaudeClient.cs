using ClaimCheck.Domain.Claims;

namespace ClaimCheck.Application.Claims;

public interface IClaudeClient
{
  Task<ClaimAnalysisResult> AnalyzeAsync(ClaimText claim, CancellationToken ct = default);
}
