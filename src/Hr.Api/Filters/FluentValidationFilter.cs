using FluentValidation;
using Hr.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hr.Api.Filters;

public class FluentValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _services;

    public FluentValidationFilter(IServiceProvider services)
    {
        _services = services;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            if (_services.GetService(validatorType) is not IValidator validator)
                continue;

            var validation = await validator.ValidateAsync(
                new ValidationContext<object>(argument),
                context.HttpContext.RequestAborted);

            if (validation.IsValid)
                continue;

            context.Result = new BadRequestObjectResult(
                new ErrorResponse(validation.Errors[0].ErrorMessage));

            return;
        }

        await next();
    }
}
