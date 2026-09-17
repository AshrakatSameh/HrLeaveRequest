namespace Hr.Api.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "Hr.Api";
    public string Audience { get; set; } = "Hr.BlazorClient";
    public string Key { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; } = 60;
}

public class AuthOptions
{
    public const string SectionName = "Auth";

    public List<AuthUser> Users { get; set; } = [];
}

public class AuthUser
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
