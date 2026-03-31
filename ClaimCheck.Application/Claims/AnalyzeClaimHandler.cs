using ClaimCheck.Domain.Claims;

namespace ClaimCheck.Application.Claims;

public sealed class AnalyzeClaimHandler
{
    private readonly IClaudeClient _claudeClient;

    public AnalyzeClaimHandler(IClaudeClient claudeClient) => _claudeClient = claudeClient;

    public async Task<ClaimAnalysisResult> HandleAsync(
        AnalyzeClaimCommand command,
        CancellationToken ct = default)
    {
        var claimText = ClaimText.Create(command.ClaimText);
        return await _claudeClient.AnalyzeAsync(claimText, ct);
    }
}
