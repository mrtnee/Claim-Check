namespace ClaimCheck.API.Claims;

public sealed record ClaimResponse(
    string[] Techniques,
    string[] CounterArguments,
    int TruthfulnessScore,
    string Explanation
);
