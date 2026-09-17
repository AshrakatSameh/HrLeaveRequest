using Hr.Application.Common;
using Hr.Application.DTO;

namespace Hr.Application.ServiceContracts;

public interface IEmployeeService
{
    Task<Result<IReadOnlyList<EmployeeResponse>>> SearchAsync(
        string? search, int limit, CancellationToken ct);

    Task<Result<EmployeeResponse>> GetByIdAsync(int id, CancellationToken ct);

    Task<(IReadOnlyDictionary<int, EmployeeResponse> Employees, bool DirectoryAvailable)>
        GetLookupAsync(IReadOnlyCollection<int> ids, CancellationToken ct);
}
