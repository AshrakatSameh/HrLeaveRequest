using Hr.Application.RepositoryContracts;
using Hr.Application.ServiceContracts;
using Hr.Infrastructure.Caching;
using Hr.Infrastructure.ExternalServices.DummyJson;
using Hr.Infrastructure.Persistence;
using Hr.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hr.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

        services.Configure<DummyJsonOptions>(
            configuration.GetSection(DummyJsonOptions.SectionName));

        services.AddHttpClient<DummyJsonEmployeeClient>((sp, http) =>
        {
            var options = sp.GetRequiredService<IOptions<DummyJsonOptions>>().Value;
            http.BaseAddress = new Uri(options.BaseUrl);
            http.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        }).SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddMemoryCache();

        services.AddScoped<IEmployeeDirectoryClient, CachedEmployeeDirectoryClient>();

        return services;
    }
}
