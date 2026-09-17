using Hr.Application.DTO;
using Hr.Application.ServiceContracts;
using Hr.Infrastructure.ExternalServices.DummyJson;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hr.Infrastructure.Caching;

public class CachedEmployeeDirectoryClient : IEmployeeDirectoryClient
{
    private const string DirectoryCacheKey = "employees:directory";

    private static readonly SemaphoreSlim Gate = new(1, 1);

    private readonly DummyJsonEmployeeClient _client;
    private readonly IMemoryCache _cache;
    private readonly DummyJsonOptions _options;

    public CachedEmployeeDirectoryClient(
        DummyJsonEmployeeClient client,
        IMemoryCache cache,
        IOptions<DummyJsonOptions> options)
    {
        _client = client;
        _cache = cache;
        _options = options.Value;
    }

    public Task<EmployeeResponse?> GetByIdAsync(int id, CancellationToken ct)
        => _client.GetByIdAsync(id, ct);

    public Task<IReadOnlyList<EmployeeResponse>> GetAllAsync(CancellationToken ct)
        => GetDirectoryAsync(ct);

    public async Task<IReadOnlyList<EmployeeResponse>> SearchAsync(
        string query, int limit, CancellationToken ct)
    {
        var directory = await GetDirectoryAsync(ct);

        if (string.IsNullOrWhiteSpace(query))
            return directory.Take(limit).ToList();

        return directory.Where(e => Matches(e, query.Trim()))
                        .Take(limit)
                        .ToList();
    }

    private async Task<IReadOnlyList<EmployeeResponse>> GetDirectoryAsync(CancellationToken ct)
    {
        if (_cache.TryGetValue(DirectoryCacheKey, out IReadOnlyList<EmployeeResponse>? cached)
            && cached is not null)
            return cached;

        await Gate.WaitAsync(ct);
        try
        {
            if (_cache.TryGetValue(DirectoryCacheKey, out cached) && cached is not null)
                return cached;

            var employees = await _client.GetAllAsync(ct);

            _cache.Set(DirectoryCacheKey, employees,
                TimeSpan.FromMinutes(_options.CacheMinutes));

            return employees;
        }
        finally
        {
            Gate.Release();
        }
    }

    private static bool Matches(EmployeeResponse employee, string query)
        => employee.FullName.Contains(query, StringComparison.OrdinalIgnoreCase)
        || (employee.Email?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
        || (employee.Department?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false);
}
