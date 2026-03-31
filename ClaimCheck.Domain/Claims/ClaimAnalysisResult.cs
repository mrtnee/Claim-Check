namespace ClaimCheck.Domain.Claims;

public sealed class ClaimAnalysisResult
{
    public string[] Techniques { get; init; } = [];
    public string[] CounterArguments { get; init; } = [];
    public int TruthfulnessScore { get; init; }
    public string Explanation { get; init; } = string.Empty;
}
