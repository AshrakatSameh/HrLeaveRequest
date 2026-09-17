using Hr.Domain.Entities;
using Hr.Domain.Enums;

namespace Hr.Application.RepositoryContracts;

public interface ILeaveRequestRepository
{
    // Items + TotalCount together
    Task<(IReadOnlyList<LeaveRequest> Items, int TotalCount)> GetPagedAsync(
        LeaveStatus? status, int? employeeId, int page, int pageSize, CancellationToken ct);
    Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken ct);

    // To check duplication before adding new request
    Task<bool> HasPendingDuplicateAsync(int employeeId, DateOnly startDate, DateOnly endDate, CancellationToken ct);
    Task AddAsync(LeaveRequest request, CancellationToken ct);
    Task UpdateAsync(LeaveRequest request, CancellationToken ct);
    Task DeleteAsync(LeaveRequest request, CancellationToken ct);
}
