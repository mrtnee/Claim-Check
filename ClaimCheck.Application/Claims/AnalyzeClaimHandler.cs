using ClaimCheck.Domain.Claims;

namespace ClaimCheck.Application.Claims;

public sealed class AnalyzeClaimHandler
{
  private readonly IClaudeClient _claudeClient;
  private readonly IClaimRepository _repository;

  public AnalyzeClaimHandler(IClaudeClient claudeClient, IClaimRepository repository)
  {
    _claudeClient = claudeClient;
    _repository = repository;
  }

  public async Task<ClaimAnalysisResult> HandleAsync(
    AnalyzeClaimCommand command,
    CancellationToken ct = default)
  {
    var claimText = ClaimText.Create(command.ClaimText);
    var result = await _claudeClient.AnalyzeAsync(claimText, ct);

    var analysis = ClaimAnalysis.Create(claimText.Value, result, command.UserId);
    await _repository.SaveAsync(analysis, ct);

    return result;
  }
}
