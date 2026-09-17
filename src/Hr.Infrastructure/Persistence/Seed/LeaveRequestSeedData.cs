using Hr.Domain.Entities;
using Hr.Domain.Enums;

namespace Hr.Infrastructure.Persistence.Seed;

public static class LeaveRequestSeedData
{
    public static readonly LeaveRequest[] Items =
    [
        new LeaveRequest
        {
            Id = 11,
            EmployeeId = 1,
            StartDate = new DateOnly(2026, 3, 2),
            EndDate = new DateOnly(2026, 3, 6),
            Type = LeaveType.Vacation,
            Status = LeaveStatus.Approved,
            CreatedAt = new DateTimeOffset(2026, 2, 10, 9, 15, 0, TimeSpan.Zero),
            ReviewerNote = "Approved; cover arranged with the team."
        },
        new LeaveRequest
        {
            Id = 12,
            EmployeeId = 2,
            StartDate = new DateOnly(2026, 3, 16),
            EndDate = new DateOnly(2026, 3, 18),
            Type = LeaveType.Sick,
            Status = LeaveStatus.Approved,
            CreatedAt = new DateTimeOffset(2026, 3, 16, 7, 40, 0, TimeSpan.Zero),
            ReviewerNote = "Medical certificate received."
        },
        new LeaveRequest
        {
            Id = 13,
            EmployeeId = 3,
            StartDate = new DateOnly(2026, 4, 6),
            EndDate = new DateOnly(2026, 4, 10),
            Type = LeaveType.Vacation,
            Status = LeaveStatus.Pending,
            CreatedAt = new DateTimeOffset(2026, 3, 20, 11, 5, 0, TimeSpan.Zero)
        },
        new LeaveRequest
        {
            Id = 4,
            EmployeeId = 4,
            StartDate = new DateOnly(2026, 4, 13),
            EndDate = new DateOnly(2026, 4, 13),
            Type = LeaveType.Sick,
            Status = LeaveStatus.Rejected,
            CreatedAt = new DateTimeOffset(2026, 4, 13, 8, 0, 0, TimeSpan.Zero),
            ReviewerNote = "No notice given and no certificate provided."
        },
        new LeaveRequest
        {
            Id = 5,
            EmployeeId = 5,
            StartDate = new DateOnly(2026, 5, 4),
            EndDate = new DateOnly(2026, 5, 15),
            Type = LeaveType.Unpaid,
            Status = LeaveStatus.Pending,
            CreatedAt = new DateTimeOffset(2026, 4, 2, 14, 30, 0, TimeSpan.Zero)
        },
        new LeaveRequest
        {
            Id = 6,
            EmployeeId = 6,
            StartDate = new DateOnly(2026, 5, 18),
            EndDate = new DateOnly(2026, 5, 20),
            Type = LeaveType.Vacation,
            Status = LeaveStatus.Pending,
            CreatedAt = new DateTimeOffset(2026, 4, 28, 10, 0, 0, TimeSpan.Zero)
        },
        new LeaveRequest
        {
            Id = 7,
            EmployeeId = 7,
            StartDate = new DateOnly(2026, 6, 1),
            EndDate = new DateOnly(2026, 6, 5),
            Type = LeaveType.Sick,
            Status = LeaveStatus.Approved,
            CreatedAt = new DateTimeOffset(2026, 6, 1, 6, 50, 0, TimeSpan.Zero),
            ReviewerNote = "Approved; workload reassigned."
        },
        new LeaveRequest
        {
            Id = 8,
            EmployeeId = 8,
            StartDate = new DateOnly(2026, 6, 22),
            EndDate = new DateOnly(2026, 6, 26),
            Type = LeaveType.Vacation,
            Status = LeaveStatus.Rejected,
            CreatedAt = new DateTimeOffset(2026, 5, 30, 16, 20, 0, TimeSpan.Zero),
            ReviewerNote = "Peak delivery week; please reschedule."
        },
        new LeaveRequest
        {
            Id = 9,
            EmployeeId = 9,
            StartDate = new DateOnly(2026, 7, 6),
            EndDate = new DateOnly(2026, 7, 17),
            Type = LeaveType.Vacation,
            Status = LeaveStatus.Pending,
            CreatedAt = new DateTimeOffset(2026, 6, 10, 9, 45, 0, TimeSpan.Zero)
        },
        new LeaveRequest
        {
            Id = 10,
            EmployeeId = 10,
            StartDate = new DateOnly(2026, 7, 20),
            EndDate = new DateOnly(2026, 7, 21),
            Type = LeaveType.Unpaid,
            Status = LeaveStatus.Pending,
            CreatedAt = new DateTimeOffset(2026, 7, 1, 13, 10, 0, TimeSpan.Zero)
        }
    ];
}
