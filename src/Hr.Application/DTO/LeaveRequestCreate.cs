using Hr.Domain.Enums;

namespace Hr.Application.DTO;
public record LeaveRequestCreate
(
    int EmployeeId,
    DateOnly StartDate,
    DateOnly EndDate,
    LeaveType Type
);
