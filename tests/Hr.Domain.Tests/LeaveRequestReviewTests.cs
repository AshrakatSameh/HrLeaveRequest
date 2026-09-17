using Hr.Domain.Entities;
using Hr.Domain.Enums;

namespace Hr.Domain.Tests;

public class LeaveRequestReviewTests
{
    private static LeaveRequest ARequest(LeaveStatus status, string? reviewerNote = null) => new()
    {
        Id = 1,
        EmployeeId = 1,
        Status = status,
        Type = LeaveType.Vacation,
        StartDate = new DateOnly(2026, 3, 2),
        EndDate = new DateOnly(2026, 3, 6),
        CreatedAt = new DateTimeOffset(2026, 1, 5, 9, 0, 0, TimeSpan.Zero),
        ReviewerNote = reviewerNote
    };

    [Fact]
    public void TryReview_approves_a_pending_request_and_trims_the_note()
    {
        var request = ARequest(LeaveStatus.Pending);

        var reviewed = request.TryReview(LeaveStatus.Approved, "  Approved by HR.  ", out var error);

        Assert.True(reviewed);
        Assert.Equal(LeaveTransitionError.None, error);
        Assert.Equal(LeaveStatus.Approved, request.Status);
        Assert.Equal("Approved by HR.", request.ReviewerNote);
    }

    [Fact]
    public void TryReview_leaves_an_already_reviewed_request_completely_untouched()
    {
        var request = ARequest(LeaveStatus.Approved, "Approved by HR.");

        var reviewed = request.TryReview(LeaveStatus.Rejected, "Changed my mind.", out var error);

        Assert.False(reviewed);
        Assert.Equal(LeaveTransitionError.AlreadyReviewed, error);
        Assert.Equal(LeaveStatus.Approved, request.Status);
        Assert.Equal("Approved by HR.", request.ReviewerNote);
    }

    [Fact]
    public void TryReview_rejects_an_unsupported_target_without_mutating()
    {
        var request = ARequest(LeaveStatus.Pending);

        var reviewed = request.TryReview(LeaveStatus.Pending, "Back to pending.", out var error);

        Assert.False(reviewed);
        Assert.Equal(LeaveTransitionError.UnsupportedTargetStatus, error);
        Assert.Equal(LeaveStatus.Pending, request.Status);
        Assert.Null(request.ReviewerNote);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void TryReview_normalises_a_blank_note_to_null(string? reviewerNote)
    {
        var request = ARequest(LeaveStatus.Pending);

        var reviewed = request.TryReview(LeaveStatus.Rejected, reviewerNote, out _);

        Assert.True(reviewed);
        Assert.Equal(LeaveStatus.Rejected, request.Status);
        Assert.Null(request.ReviewerNote);
    }
}
