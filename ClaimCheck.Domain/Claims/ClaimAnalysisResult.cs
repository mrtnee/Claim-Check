namespace ClaimCheck.Domain.Claims;

public sealed record ClaimAnalysisResult(
    string[] Techniques,
    string[] CounterArguments,
    int TruthfulnessScore,
    string Explanation
);
