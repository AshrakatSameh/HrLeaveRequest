namespace Hr.BlazorClient.Models;

public enum LeaveType
{
    Vacation,
    Sick,
    Unpaid
}

public enum LeaveStatus
{
    Pending,
    Approved,
    Rejected
}

public record EmployeeResponse(
    int Id,
    string FirstName,
    string LastName,
    string? Email,
    string? Department,
    string? Title)
{
    public string FullName => $"{FirstName} {LastName}".Trim();
}

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
    string? ReviewerNote)
{
    public int TotalDays => (EndDate.DayNumber - StartDate.DayNumber) + 1;
}

public record LeaveRequestCreate(
    int EmployeeId,
    DateOnly StartDate,
    DateOnly EndDate,
    LeaveType Type);

public record LeaveRequestStatusUpdate(
    LeaveStatus Status,
    string? ReviewerNote);

public record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool EmployeeDirectoryAvailable);

public record ErrorResponse(string Error);

public record LoginRequest(string Username, string Password);

public record LoginResponse(
    string Token,
    string Username,
    string Role,
    DateTimeOffset ExpiresAt);
