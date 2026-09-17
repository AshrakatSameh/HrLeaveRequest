using Hr.Application.Common;
using Hr.Application.DTO;

namespace Hr.Application.ServiceContracts;

// For managing leave requests
public interface ILeaveRequestService
{
    Task<Result<PagedResponse<LeaveRequestResponse>>> GetAllAsync(
        LeaveRequestQuery query, CancellationToken ct);

    Task<Result<LeaveRequestResponse>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result<LeaveRequestResponse>> CreateAsync(
        LeaveRequestCreate request, CancellationToken ct);

    Task<Result<LeaveRequestResponse>> UpdateStatusAsync(
        int id, LeaveRequestStatusUpdate request, CancellationToken ct);

    Task<Result> DeleteAsync(int id, CancellationToken ct);
}
