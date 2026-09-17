using Hr.Application.RepositoryContracts;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using Hr.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hr.Infrastructure.Repositories;

public class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly AppDbContext _db;

    public LeaveRequestRepository(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<LeaveRequest> Items, int TotalCount)> GetPagedAsync(
        LeaveStatus? status, int? employeeId, int page, int pageSize, CancellationToken ct)
    {
        IQueryable<LeaveRequest> query = _db.LeaveRequests.AsNoTracking();

        if (status is not null) query = query.Where(x => x.Status == status);
        if (employeeId is not null) query = query.Where(x => x.EmployeeId == employeeId);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)   
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken ct)
        => await _db.LeaveRequests.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<bool> HasPendingDuplicateAsync(
        int employeeId, DateOnly startDate, DateOnly endDate, CancellationToken ct)
        => await _db.LeaveRequests.AnyAsync(
            x => x.EmployeeId == employeeId
              && x.StartDate == startDate
              && x.EndDate == endDate
              && x.Status == LeaveStatus.Pending,
            ct);

    public async Task AddAsync(LeaveRequest request, CancellationToken ct)
    {
        _db.LeaveRequests.Add(request);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(LeaveRequest request, CancellationToken ct)
        => await _db.SaveChangesAsync(ct);

    public async Task DeleteAsync(LeaveRequest request, CancellationToken ct)
    {
        _db.LeaveRequests.Remove(request);
        await _db.SaveChangesAsync(ct);
    }
}
