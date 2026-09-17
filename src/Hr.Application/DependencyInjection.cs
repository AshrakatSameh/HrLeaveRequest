using FluentValidation;
using Hr.Application.ServiceContracts;
using Hr.Application.Services;
using Hr.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LeaveRequestCreateValidator>(
            ServiceLifetime.Scoped);

        services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}
