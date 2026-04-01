namespace ClaimCheck.Infrastructure.Auth;

public sealed class JwtOptions
{
  public const string SectionName = "Jwt";

  public string Key { get; set; } = string.Empty;
  public string Issuer { get; set; } = string.Empty;
  public string Audience { get; set; } = string.Empty;
  public int ExpiryHours { get; set; } = 24;
}
