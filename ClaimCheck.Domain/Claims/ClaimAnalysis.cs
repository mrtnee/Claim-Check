namespace ClaimCheck.Domain.Claims;

public sealed class ClaimAnalysis
{
    public Guid Id { get; private set; }
    public string ClaimText { get; private set; } = string.Empty;
    public ClaimAnalysisResult Result { get; private set; } = null!;
    public DateTime AnalyzedAt { get; private set; }
    public string UserId { get; private set; } = string.Empty;

    private ClaimAnalysis() { } // EF Core constructor

    public static ClaimAnalysis Create(string claimText, ClaimAnalysisResult result, string userId) =>
        new()
        {
            Id = Guid.NewGuid(),
            ClaimText = claimText,
            Result = result,
            AnalyzedAt = DateTime.UtcNow,
            UserId = userId
        };
}
