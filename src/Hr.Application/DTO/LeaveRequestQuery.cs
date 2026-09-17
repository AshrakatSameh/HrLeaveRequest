using Hr.Domain.Enums;

namespace Hr.Application.DTO;
public record LeaveRequestQuery
{
    public LeaveStatus? Status { get; init; }
    public int? EmployeeId { get; init; }    
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
