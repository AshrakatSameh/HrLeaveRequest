using FluentValidation;
using Hr.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LeaveRequestCreateValidator>(
            ServiceLifetime.Scoped);

        return services;
    }
}
