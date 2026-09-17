using Hr.Api.Contracts;
using Hr.Application.Common;

namespace Hr.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EmployeeDirectoryUnavailableException ex)
        {
            _logger.LogWarning(ex, "The employee directory was unavailable.");

            if (context.Response.HasStarted)
                throw;

            await WriteAsync(context, StatusCodes.Status502BadGateway, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing {Path}.",
                context.Request.Path);

            if (context.Response.HasStarted)
                throw;

            var message = _environment.IsDevelopment()
                ? ex.Message
                : "An unexpected error occurred.";

            await WriteAsync(context, StatusCodes.Status500InternalServerError, message);
        }
    }

    private static Task WriteAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(new ErrorResponse(message));
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
