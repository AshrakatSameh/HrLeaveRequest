using FluentValidation;
using Hr.Application.DTO;
using Hr.Domain.Rules;

namespace Hr.Application.Validators;

public sealed class LeaveRequestStatusUpdateValidator
    : AbstractValidator<LeaveRequestStatusUpdate>
{
    public LeaveRequestStatusUpdateValidator()
    {
        RuleFor(x => x.Status)
            .Must(LeaveStatusTransition.AllowedTargets.Contains)
            .WithMessage("Status must be Approved or Rejected.");

        RuleFor(x => x.ReviewerNote)
            .MaximumLength(500)
            .WithMessage("ReviewerNote cannot exceed 500 characters.");
    }
}
