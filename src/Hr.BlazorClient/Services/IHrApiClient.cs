using Hr.BlazorClient.Models;

namespace Hr.BlazorClient.Services;

public interface IHrApiClient
{
    Task<ApiResult<PagedResponse<LeaveRequestResponse>>> GetLeaveRequestsAsync(
        LeaveStatus? status, int page, int pageSize, CancellationToken ct = default);

    Task<ApiResult<LeaveRequestResponse>> CreateLeaveRequestAsync(
        LeaveRequestCreate request, CancellationToken ct = default);

    Task<ApiResult<LeaveRequestResponse>> UpdateStatusAsync(
        int id, LeaveRequestStatusUpdate request, CancellationToken ct = default);

    Task<ApiResult> DeleteLeaveRequestAsync(int id, CancellationToken ct = default);

    Task<ApiResult<IReadOnlyList<EmployeeResponse>>> GetEmployeesAsync(
        string? search = null, int limit = 200, CancellationToken ct = default);

    Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
