namespace ClaimCheck.Domain.Claims;

public sealed class ClaimText
{
  public string Value { get; }

  private ClaimText(string value) => Value = value;

  public static ClaimText Create(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      throw new ArgumentException("Claim text cannot be empty.", nameof(value));
    return new ClaimText(value.Trim());
  }
}
