using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hr.BlazorClient.Models;

namespace Hr.BlazorClient.Services;

public class HrApiClient : IHrApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _http;

    public HrApiClient(HttpClient http)
    {
        _http = http;
    }

    public Task<ApiResult<PagedResponse<LeaveRequestResponse>>> GetLeaveRequestsAsync(
        LeaveStatus? status, int page, int pageSize, CancellationToken ct = default)
    {
        var url = $"api/leave-requests?page={page}&pageSize={pageSize}";

        if (status is not null)
            url += $"&status={status}";

        return SendAsync<PagedResponse<LeaveRequestResponse>>(() => _http.GetAsync(url, ct), ct);
    }

    public Task<ApiResult<LeaveRequestResponse>> CreateLeaveRequestAsync(
        LeaveRequestCreate request, CancellationToken ct = default)
        => SendAsync<LeaveRequestResponse>(
            () => _http.PostAsJsonAsync("api/leave-requests", request, JsonOptions, ct), ct);

    public Task<ApiResult<LeaveRequestResponse>> UpdateStatusAsync(
        int id, LeaveRequestStatusUpdate request, CancellationToken ct = default)
        => SendAsync<LeaveRequestResponse>(
            () => _http.PutAsJsonAsync($"api/leave-requests/{id}/status", request, JsonOptions, ct), ct);

    public async Task<ApiResult> DeleteLeaveRequestAsync(int id, CancellationToken ct = default)
    {
        try
        {
            using var response = await _http.DeleteAsync($"api/leave-requests/{id}", ct);

            return response.IsSuccessStatusCode
                ? ApiResult.Success()
                : ApiResult.Failure(await ReadErrorAsync(response, ct));
        }
        catch (HttpRequestException)
        {
            return ApiResult.Failure(UnreachableMessage);
        }
    }

    public Task<ApiResult<IReadOnlyList<EmployeeResponse>>> GetEmployeesAsync(
        string? search = null, int limit = 200, CancellationToken ct = default)
    {
        var url = $"api/employees?limit={limit}";

        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";

        return SendAsync<IReadOnlyList<EmployeeResponse>>(() => _http.GetAsync(url, ct), ct);
    }

    public Task<ApiResult<LoginResponse>> LoginAsync(
        LoginRequest request, CancellationToken ct = default)
        => SendAsync<LoginResponse>(
            () => _http.PostAsJsonAsync("api/auth/login", request, JsonOptions, ct), ct);

    private async Task<ApiResult<T>> SendAsync<T>(
        Func<Task<HttpResponseMessage>> send, CancellationToken ct)
    {
        try
        {
            using var response = await send();

            if (!response.IsSuccessStatusCode)
                return ApiResult<T>.Failure(await ReadErrorAsync(response, ct));

            var value = await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);

            return value is null
                ? ApiResult<T>.Failure("The API returned an empty response.")
                : ApiResult<T>.Success(value);
        }
        catch (HttpRequestException)
        {
            return ApiResult<T>.Failure(UnreachableMessage);
        }
        catch (JsonException)
        {
            return ApiResult<T>.Failure("The API returned a response the client could not read.");
        }
    }

    private static async Task<string> ReadErrorAsync(
        HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions, ct);

            if (!string.IsNullOrWhiteSpace(error?.Error))
                return error.Error;
        }
        catch (JsonException)
        {
        }

        return $"The request failed with status {(int)response.StatusCode}.";
    }

    private const string UnreachableMessage =
        "Could not reach the API. Make sure it is running and that its certificate is trusted.";
}
