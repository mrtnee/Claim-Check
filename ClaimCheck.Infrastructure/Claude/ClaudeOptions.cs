namespace ClaimCheck.Infrastructure.Claude;

public sealed class ClaudeOptions
{
  public const string SectionName = "Anthropic";

  public string ApiKey { get; set; } = string.Empty;
  public string Model { get; set; } = "claude-haiku-4-5-20251001";
  public int MaxTokens { get; set; } = 1024;
}
