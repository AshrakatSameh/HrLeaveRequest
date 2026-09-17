namespace Hr.Api.Contracts;

public record LoginRequest(string Username, string Password);

public record LoginResponse(
    string Token,
    string Username,
    string Role,
    DateTimeOffset ExpiresAt);
