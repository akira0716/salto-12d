using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Repositories;
using LoanManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class LoanRequestRepository : ILoanRequestRepository
{
    private readonly LoanDbContext _context;

    public LoanRequestRepository(LoanDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LoanRequest request)
    {
        _context.LoanRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<LoanRequest>> GetAllAsync(int? userId = null, LoanRequestStatus? status = null)
    {
        var query = _context.LoanRequests.AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(r => r.UserId == userId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        return await query.OrderByDescending(r => r.RequestDate).ToListAsync();
    }

    public async Task<LoanRequest?> GetByIdAsync(int id)
    {
        return await _context.LoanRequests.FindAsync(id);
    }

    public async Task<IEnumerable<LoanRequest>> GetPendingRequestsByUserIdAsync(int userId)
    {
        return await _context.LoanRequests
            .Where(r => r.UserId == userId && r.Status == LoanRequestStatus.Pending)
            .ToListAsync();
    }

    public async Task UpdateAsync(LoanRequest request)
    {
        _context.LoanRequests.Update(request);
        await _context.SaveChangesAsync();
    }
}
