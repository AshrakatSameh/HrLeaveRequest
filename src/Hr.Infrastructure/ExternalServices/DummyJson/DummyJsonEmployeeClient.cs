using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Hr.Application.Common;
using Hr.Application.DTO;

namespace Hr.Infrastructure.ExternalServices.DummyJson;

public class DummyJsonEmployeeClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private const string SelectFields = "firstName,lastName,email,company";

    private readonly HttpClient _http;

    public DummyJsonEmployeeClient(HttpClient http)
    {
        _http = http;
    }

    public Task<EmployeeResponse?> GetByIdAsync(int id, CancellationToken ct)
        => SendAsync<EmployeeResponse?>(async () =>
        {
            using var response = await _http.GetAsync($"users/{id}", ct);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (!response.IsSuccessStatusCode)
                throw new EmployeeDirectoryUnavailableException(
                    $"Employee directory returned HTTP {(int)response.StatusCode}.");

            var user = await response.Content.ReadFromJsonAsync<DummyJsonUser>(JsonOptions, ct);
            return user is null ? null : ToEmployee(user);
        }, ct);

    public Task<IReadOnlyList<EmployeeResponse>> GetAllAsync(CancellationToken ct)
        => SendAsync<IReadOnlyList<EmployeeResponse>>(async () =>
        {
            using var response = await _http.GetAsync($"users?limit=0&select={SelectFields}", ct);

            if (!response.IsSuccessStatusCode)
                throw new EmployeeDirectoryUnavailableException(
                    $"Employee directory returned HTTP {(int)response.StatusCode}.");

            var payload = await response.Content
                .ReadFromJsonAsync<DummyJsonUserListResponse>(JsonOptions, ct);

            return payload?.Users.Select(ToEmployee).ToList() ?? [];
        }, ct);

    private static async Task<T> SendAsync<T>(Func<Task<T>> send, CancellationToken ct)
    {
        try
        {
            return await send();
        }
        catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
        {
            throw new EmployeeDirectoryUnavailableException(
                "The employee directory did not respond in time.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new EmployeeDirectoryUnavailableException(
                "The employee directory is unreachable.", ex);
        }
        catch (JsonException ex)
        {
            throw new EmployeeDirectoryUnavailableException(
                "The employee directory returned an unexpected response.", ex);
        }
    }

    private static EmployeeResponse ToEmployee(DummyJsonUser user) => new(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        user.Company?.Department,
        user.Company?.Title
        );
}
