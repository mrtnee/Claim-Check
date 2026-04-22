using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ClaimCheck.Web.Services;

public interface IAuthService
{
  Task<string[]> RegisterAsync(string email, string password);
  Task<bool> LoginAsync(string email, string password);
  Task LogoutAsync();
  Task<string?> GetTokenAsync();
}

public sealed class AuthService : IAuthService
{
  private const string TokenKey = "authToken";

  private readonly HttpClient _http;
  private readonly IJSRuntime _js;
  private readonly IServiceProvider _sp;

  public AuthService(HttpClient http, IJSRuntime js, IServiceProvider sp)
  {
    _http = http;
    _js = js;
    _sp = sp;
  }

  public async Task<string[]> RegisterAsync(string email, string password)
  {
    var response = await _http.PostAsJsonAsync("api/auth/register", new { email, password });

    if (response.IsSuccessStatusCode)
      return [];

    try
    {
      var errors = await response.Content.ReadFromJsonAsync<string[]>();
      return errors ?? ["Registration failed."];
    }
    catch
    {
      return [$"Registration failed (HTTP {(int)response.StatusCode})."];
    }
  }

  public async Task<bool> LoginAsync(string email, string password)
  {
    var response = await _http.PostAsJsonAsync("api/auth/login", new { email, password });

    if (!response.IsSuccessStatusCode)
      return false;

    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
    if (result?.Token is null)
      return false;

    await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
    NotifyChanged();
    return true;
  }

  public async Task LogoutAsync()
  {
    await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    NotifyChanged();
  }

  public async Task<string?> GetTokenAsync() =>
    await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

  private void NotifyChanged()
  {
    var provider = (JwtAuthStateProvider)_sp.GetRequiredService<AuthenticationStateProvider>();
    provider.NotifyChanged();
  }

  private sealed record LoginResponse(string Token);
}
