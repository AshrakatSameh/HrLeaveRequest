using System.Text.Json;
using Microsoft.JSInterop;

namespace Hr.BlazorClient.Services;

public class AuthState
{
    private const string StorageKey = "hr.auth";

    private readonly IJSRuntime _js;

    public AuthState(IJSRuntime js)
    {
        _js = js;
    }

    public string? Token { get; private set; }
    public string? Username { get; private set; }
    public string? Role { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    public bool IsHr => string.Equals(Role, "HR", StringComparison.OrdinalIgnoreCase);

    public event Action? Changed;

    public async Task InitializeAsync()
    {
        var json = await ReadAsync();

        if (string.IsNullOrWhiteSpace(json))
            return;

        try
        {
            var session = JsonSerializer.Deserialize<StoredSession>(json);

            if (session is null || session.ExpiresAt <= DateTimeOffset.UtcNow)
            {
                await SignOutAsync();
                return;
            }

            Token = session.Token;
            Username = session.Username;
            Role = session.Role;
        }
        catch (JsonException)
        {
        }
    }

    public async Task SignInAsync(string token, string username, string role, DateTimeOffset expiresAt)
    {
        Token = token;
        Username = username;
        Role = role;

        var json = JsonSerializer.Serialize(new StoredSession(token, username, role, expiresAt));

        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch (JSException)
        {
        }

        Changed?.Invoke();
    }

    public async Task SignOutAsync()
    {
        Token = null;
        Username = null;
        Role = null;

        try
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        }
        catch (JSException)
        {
        }

        Changed?.Invoke();
    }

    private async Task<string?> ReadAsync()
    {
        try
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        }
        catch (JSException)
        {
            return null;
        }
    }

    private sealed record StoredSession(
        string Token, string Username, string Role, DateTimeOffset ExpiresAt);
}
