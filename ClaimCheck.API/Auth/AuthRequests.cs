namespace ClaimCheck.API.Auth;

public sealed record RegisterRequest(string Email, string Password);
public sealed record LoginRequest(string Email, string Password);
public sealed record LoginResponse(string Token);
