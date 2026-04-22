namespace ClaimCheck.Web.Models;

public sealed record ClaimResultModel(
    string[] Techniques,
    string[] CounterArguments,
    int TruthfulnessScore,
    string Explanation
);
