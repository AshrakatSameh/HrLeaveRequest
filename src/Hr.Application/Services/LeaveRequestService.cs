using Hr.Application.Common;
using Hr.Application.DTO;
using Hr.Application.RepositoryContracts;
using Hr.Application.ServiceContracts;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using Hr.Domain.Rules;

namespace Hr.Application.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestRepository _repository;
    private readonly IEmployeeService _employees;

    public LeaveRequestService(ILeaveRequestRepository repository, IEmployeeService employees)
    {
        _repository = repository;
        _employees = employees;
    }

    public async Task<Result<PagedResponse<LeaveRequestResponse>>> GetAllAsync(
        LeaveRequestQuery query, CancellationToken ct)
    {
        var (entities, totalCount) = await _repository.GetPagedAsync(
            query.Status, query.EmployeeId, query.Page, query.PageSize, ct);

        var items = entities.Select(ToResponse).ToList();

        var ids = items.Select(x => x.EmployeeId).Distinct().ToList();
        var (lookup, directoryAvailable) = await _employees.GetLookupAsync(ids, ct);

        var enriched = items
            .Select(item => lookup.TryGetValue(item.EmployeeId, out var employee)
                ? WithEmployee(item, employee)
                : item)
            .ToList();

        var response = new PagedResponse<LeaveRequestResponse>(
            enriched, query.Page, query.PageSize, totalCount, directoryAvailable);

        return Result<PagedResponse<LeaveRequestResponse>>.Success(response);
    }

    public async Task<Result<LeaveRequestResponse>> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);

        if (entity is null)
            return Result<LeaveRequestResponse>.NotFound($"Leave request {id} was not found.");

        return Result<LeaveRequestResponse>.Success(await EnrichAsync(entity, ct));
    }

    public async Task<Result<LeaveRequestResponse>> CreateAsync(
        LeaveRequestCreate request, CancellationToken ct)
    {
        var employeeResult = await _employees.GetByIdAsync(request.EmployeeId, ct);

        if (!employeeResult.IsSuccess)
            return employeeResult.ErrorType == ErrorType.NotFound
                ? Result<LeaveRequestResponse>.NotFound(employeeResult.Error!)
                : Result<LeaveRequestResponse>.Upstream(employeeResult.Error!);

        var hasDuplicate = await _repository.HasPendingDuplicateAsync(
            request.EmployeeId, request.StartDate, request.EndDate, ct);

        if (hasDuplicate)
            return Result<LeaveRequestResponse>.Conflict(
                "A pending leave request for this employee and date range already exists.");

        var entity = new LeaveRequest
        {
            EmployeeId = request.EmployeeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type,
            Status = LeaveStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(entity, ct);

        var response = WithEmployee(ToResponse(entity), employeeResult.Value);
        return Result<LeaveRequestResponse>.Success(response);
    }

    public async Task<Result<LeaveRequestResponse>> UpdateStatusAsync(
        int id, LeaveRequestStatusUpdate request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);

        if (entity is null)
            return Result<LeaveRequestResponse>.NotFound($"Leave request {id} was not found.");

        if (!entity.TryReview(request.Status, request.ReviewerNote, out var error))
            return error == LeaveTransitionError.UnsupportedTargetStatus
                ? Result<LeaveRequestResponse>.Validation("Status must be Approved or Rejected.")
                : Result<LeaveRequestResponse>.Conflict(
                    $"Leave request {id} is already {entity.Status}; " +
                    "only pending requests can be reviewed.");

        await _repository.UpdateAsync(entity, ct);

        return Result<LeaveRequestResponse>.Success(await EnrichAsync(entity, ct));
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);

        if (entity is null)
            return Result.NotFound($"Leave request {id} was not found.");

        if (!LeaveStatusTransition.CanDelete(entity.Status))
            return Result.Validation(
                $"Only pending leave requests can be deleted; this one is {entity.Status}.");

        await _repository.DeleteAsync(entity, ct);

        return Result.Success();
    }

    private async Task<LeaveRequestResponse> EnrichAsync(LeaveRequest entity, CancellationToken ct)
    {
        var response = ToResponse(entity);
        var (lookup, _) = await _employees.GetLookupAsync(new[] { entity.EmployeeId }, ct);

        return lookup.TryGetValue(entity.EmployeeId, out var employee)
            ? WithEmployee(response, employee)
            : response;
    }

    private static LeaveRequestResponse ToResponse(LeaveRequest entity) => new(
        entity.Id,
        entity.EmployeeId,
        EmployeeName: null,
        EmployeeDepartment: null,
        EmployeeTitle: null,
        entity.StartDate,
        entity.EndDate,
        entity.Type,
        entity.Status,
        entity.CreatedAt,
        entity.ReviewerNote);

    private static LeaveRequestResponse WithEmployee(
        LeaveRequestResponse response, EmployeeResponse? employee)
        => employee is null
            ? response
            : response with
            {
                EmployeeName = employee.FullName,
                EmployeeDepartment = employee.Department,
                EmployeeTitle = employee.Title
            };
}
