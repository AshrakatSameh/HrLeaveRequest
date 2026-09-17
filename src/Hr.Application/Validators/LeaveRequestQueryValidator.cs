using FluentValidation;
using Hr.Application.DTO;

namespace Hr.Application.Validators;

public sealed class LeaveRequestQueryValidator : AbstractValidator<LeaveRequestQuery>
{
    public LeaveRequestQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be 1 or greater.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}
