using Hr.Application.Common;
using Hr.Application.DTO;
using Hr.Application.ServiceContracts;

namespace Hr.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeDirectoryClient _directory;

    public EmployeeService(IEmployeeDirectoryClient directory)
    {
        _directory = directory;
    }

    public async Task<Result<IReadOnlyList<EmployeeResponse>>> SearchAsync(
        string? search, int limit, CancellationToken ct)
    {
        try
        {
            var employees = await _directory.SearchAsync(search ?? string.Empty, limit, ct);
            return Result<IReadOnlyList<EmployeeResponse>>.Success(employees);
        }
        catch (EmployeeDirectoryUnavailableException ex)
        {
            return Result<IReadOnlyList<EmployeeResponse>>.Upstream(ex.Message);
        }
    }

    public async Task<Result<EmployeeResponse>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var employee = await _directory.GetByIdAsync(id, ct);

            return employee is null
                ? Result<EmployeeResponse>.NotFound(
                    $"Employee {id} was not found in the employee directory.")
                : Result<EmployeeResponse>.Success(employee);
        }
        catch (EmployeeDirectoryUnavailableException ex)
        {
            return Result<EmployeeResponse>.Upstream(ex.Message);
        }
    }

    public async Task<(IReadOnlyDictionary<int, EmployeeResponse> Employees, bool DirectoryAvailable)>
        GetLookupAsync(IReadOnlyCollection<int> ids, CancellationToken ct)
    {
        var empty = new Dictionary<int, EmployeeResponse>();

        if (ids.Count == 0)
            return (empty, true);

        try
        {
            var directory = await _directory.GetAllAsync(ct);
            var wanted = ids.ToHashSet();

            var matched = directory
                .Where(e => wanted.Contains(e.Id))
                .ToDictionary(e => e.Id);

            return (matched, true);
        }
        catch (EmployeeDirectoryUnavailableException)
        {
            return (empty, false);
        }
    }
}
