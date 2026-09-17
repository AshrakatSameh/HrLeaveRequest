using Hr.Domain.Enums;

namespace Hr.Application.DTO;
public record LeaveRequestStatusUpdate
(
    LeaveStatus Status,
    string? ReviewerNote

    );

