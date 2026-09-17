using Hr.Domain.Enums;
using Hr.Domain.Rules;

namespace Hr.Domain.Tests;

public class LeaveStatusTransitionTests
{
    [Theory]
    [InlineData(LeaveStatus.Pending, LeaveStatus.Approved)]
    [InlineData(LeaveStatus.Pending, LeaveStatus.Rejected)]
    public void CanTransition_allows_reviewing_a_pending_request(
        LeaveStatus current, LeaveStatus target)
    {
        var allowed = LeaveStatusTransition.CanTransition(current, target, out var error);

        Assert.True(allowed);
        Assert.Equal(LeaveTransitionError.None, error);
    }

    [Theory]
    [InlineData(LeaveStatus.Approved, LeaveStatus.Approved)]
    [InlineData(LeaveStatus.Approved, LeaveStatus.Rejected)]
    [InlineData(LeaveStatus.Rejected, LeaveStatus.Approved)]
    [InlineData(LeaveStatus.Rejected, LeaveStatus.Rejected)]
    [InlineData((LeaveStatus)99, LeaveStatus.Approved)]
    public void CanTransition_rejects_an_already_reviewed_request(
        LeaveStatus current, LeaveStatus target)
    {
        var allowed = LeaveStatusTransition.CanTransition(current, target, out var error);

        Assert.False(allowed);
        Assert.Equal(LeaveTransitionError.AlreadyReviewed, error);
    }

    [Theory]
    [InlineData(LeaveStatus.Pending, LeaveStatus.Pending)]
    [InlineData(LeaveStatus.Approved, LeaveStatus.Pending)]   
    [InlineData(LeaveStatus.Rejected, LeaveStatus.Pending)]
    [InlineData(LeaveStatus.Pending, (LeaveStatus)99)]
    public void CanTransition_rejects_an_unsupported_target(
        LeaveStatus current, LeaveStatus target)
    {
        var allowed = LeaveStatusTransition.CanTransition(current, target, out var error);

        Assert.False(allowed);
        Assert.Equal(LeaveTransitionError.UnsupportedTargetStatus, error);
    }

    [Fact]
    public void CanTransition_allows_exactly_two_of_every_possible_pair()
    {
        var statuses = Enum.GetValues<LeaveStatus>();
        var allowed = new List<(LeaveStatus Current, LeaveStatus Target)>();

        foreach (var current in statuses)
            foreach (var target in statuses)
                if (LeaveStatusTransition.CanTransition(current, target, out _))
                    allowed.Add((current, target));

        Assert.Equal(2, allowed.Count);
        Assert.Contains((LeaveStatus.Pending, LeaveStatus.Approved), allowed);
        Assert.Contains((LeaveStatus.Pending, LeaveStatus.Rejected), allowed);
    }

    [Theory]
    [InlineData(LeaveStatus.Pending, true)]
    [InlineData(LeaveStatus.Approved, false)]
    [InlineData(LeaveStatus.Rejected, false)]
    [InlineData((LeaveStatus)99, false)]
    public void CanDelete_allows_only_a_pending_request(LeaveStatus current, bool expected)
    {
        Assert.Equal(expected, LeaveStatusTransition.CanDelete(current));
    }
}
