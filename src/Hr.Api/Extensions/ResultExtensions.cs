using Hr.Api.Contracts;
using Hr.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToErrorResult(this Result result)
        => Build(result.ErrorType, result.Error);

    public static IActionResult ToErrorResult<T>(this Result<T> result)
        => Build(result.ErrorType, result.Error);

    private static IActionResult Build(ErrorType errorType, string? error)
    {
        var body = new ErrorResponse(error ?? "An unexpected error occurred.");

        return errorType switch
        {
            ErrorType.Validation => new BadRequestObjectResult(body),
            ErrorType.NotFound => new NotFoundObjectResult(body),
            ErrorType.Conflict => new ConflictObjectResult(body),
            ErrorType.Upstream => new ObjectResult(body)
            {
                StatusCode = StatusCodes.Status502BadGateway
            },
            _ => new ObjectResult(body)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}
