using Hr.Domain.Enums;

namespace Hr.Domain.Rules;
public static class LeaveStatusTransition
{
    public static readonly IReadOnlySet<LeaveStatus> AllowedTargets = new HashSet<LeaveStatus>
    {
        LeaveStatus.Approved,
        LeaveStatus.Rejected
    };
    public static bool CanTransition(LeaveStatus currentStatus, LeaveStatus targetStatus, out LeaveTransitionError error)
    {
        if(!Enum.IsDefined(targetStatus) || !AllowedTargets.Contains(targetStatus))
        {
            error = LeaveTransitionError.UnsupportedTargetStatus;
            return false;
        }
        if(!Enum.IsDefined(currentStatus) || currentStatus != LeaveStatus.Pending)
        {
            error = LeaveTransitionError.AlreadyReviewed;
            return false;
        }
        error = LeaveTransitionError.None;
        return true;
    }

    public static bool CanDelete(LeaveStatus currentStatus)
    {
        return currentStatus == LeaveStatus.Pending;
    }


}
