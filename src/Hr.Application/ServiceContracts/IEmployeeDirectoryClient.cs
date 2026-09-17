using Hr.Application.DTO;

namespace Hr.Application.ServiceContracts;

// For fetching employee data from the Employee Directory service 
public interface IEmployeeDirectoryClient
{
    Task<EmployeeResponse?> GetByIdAsync(int id, CancellationToken ct);

    Task<IReadOnlyList<EmployeeResponse>> GetAllAsync(CancellationToken ct);

    Task<IReadOnlyList<EmployeeResponse>> SearchAsync(string query, int limit, CancellationToken ct);
}
