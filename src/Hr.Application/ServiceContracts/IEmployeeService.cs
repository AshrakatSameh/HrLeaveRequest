using Hr.Application.Common;
using Hr.Application.DTO;

namespace Hr.Application.ServiceContracts;

public interface IEmployeeService
{
    Task<Result<IReadOnlyList<EmployeeResponse>>> SearchAsync(
        string? search, int limit, CancellationToken ct);

    Task<IReadOnlyDictionary<int, EmployeeResponse>> GetLookupAsync(
        IReadOnlyCollection<int> ids, CancellationToken ct);
}
