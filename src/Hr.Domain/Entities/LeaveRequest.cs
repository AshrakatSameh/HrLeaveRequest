using Hr.Domain.Enums;
using Hr.Domain.Rules;

namespace Hr.Domain.Entities;
public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public LeaveStatus Status { get; set; }
    public LeaveType Type { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? ReviewerNote { get; set; }

    public bool TryReview(LeaveStatus targetStatus, string? reviewerNote,
                          out LeaveTransitionError error)
    {
        if (!LeaveStatusTransition.CanTransition(Status, targetStatus, out error))
            return false;

        Status = targetStatus;
        ReviewerNote = string.IsNullOrWhiteSpace(reviewerNote)
            ? null
            : reviewerNote.Trim();
        return true;
    }

}

