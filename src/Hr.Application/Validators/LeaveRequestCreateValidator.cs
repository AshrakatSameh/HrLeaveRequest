using FluentValidation;
using Hr.Application.DTO;

namespace Hr.Application.Validators;

public sealed class LeaveRequestCreateValidator : AbstractValidator<LeaveRequestCreate>
{
    public LeaveRequestCreateValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("EmployeeId must be greater than zero.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be Vacation, Sick or Unpaid.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("EndDate must be on or after StartDate.");
    }
}
