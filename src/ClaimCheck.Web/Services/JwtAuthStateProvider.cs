using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClaimCheck.Web.Services;

public sealed class JwtAuthStateProvider : AuthenticationStateProvider
{
  private readonly IAuthService _authService;
  private static readonly AuthenticationState Anonymous =
    new(new ClaimsPrincipal(new ClaimsIdentity()));

  public JwtAuthStateProvider(IAuthService authService) =>
    _authService = authService;

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var token = await _authService.GetTokenAsync();
    if (string.IsNullOrWhiteSpace(token))
      return Anonymous;

    var claims = ParseClaimsFromJwt(token).ToList();

    var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
    if (expClaim is not null && long.TryParse(expClaim.Value, out var expUnix))
    {
      if (DateTimeOffset.FromUnixTimeSeconds(expUnix) < DateTimeOffset.UtcNow)
      {
        await _authService.LogoutAsync();
        return Anonymous;
      }
    }

    var identity = new ClaimsIdentity(claims, "jwt");
    return new AuthenticationState(new ClaimsPrincipal(identity));
  }

  public void NotifyChanged() =>
    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

  private static IEnumerable<Claim> ParseClaimsFromJwt(string token)
  {
    var payload = token.Split('.')[1];
    var padded = (payload.Length % 4) switch
    {
      2 => payload + "==",
      3 => payload + "=",
      _ => payload
    };
    var bytes = Convert.FromBase64String(
      padded.Replace('-', '+').Replace('_', '/'));

    using var doc = JsonDocument.Parse(Encoding.UTF8.GetString(bytes));
    return doc.RootElement.EnumerateObject()
      .Select(p => new Claim(p.Name, p.Value.ToString()))
      .ToList();
  }
}
