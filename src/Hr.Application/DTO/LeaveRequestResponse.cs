using Hr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTO;
public record LeaveRequestResponse(

    int Id,
    int EmployeeId,
    string? EmployeeName,
    string? EmployeeDepartment,
    string? EmployeeTitle,
    DateOnly StartDate,
    DateOnly EndDate,
    LeaveType Type,
    LeaveStatus Status,
    DateTimeOffset CreatedAt,
    string? ReviewerNote
)
{
    public int TotalDays => (EndDate.DayNumber - StartDate.DayNumber) + 1;
}

