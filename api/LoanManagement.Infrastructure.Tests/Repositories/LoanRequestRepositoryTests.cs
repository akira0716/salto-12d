using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Infrastructure.Persistence;
using LoanManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LoanManagement.Infrastructure.Tests.Repositories;

public class LoanRequestRepositoryTests
{
    private DbContextOptions<LoanDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<LoanDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetPendingRequestsByUserIdAsync_ShouldReturnOnlyPending()
    {
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            var req1 = new LoanRequest(1, 1, 1, DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "Purpose");
            var req2 = new LoanRequest(2, 1, 2, DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "Purpose 2");
            req2.Approve();
            context.LoanRequests.Add(req1);
            context.LoanRequests.Add(req2);
            await context.SaveChangesAsync();
        }

        using (var context = new LoanDbContext(options))
        {
            var repository = new LoanRequestRepository(context);
            var result = await repository.GetPendingRequestsByUserIdAsync(1);

            Assert.Single(result);
            Assert.Equal(LoanRequestStatus.Pending, result.First().Status);
            Assert.Equal(1, result.First().Id);
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldFilterByUserIdAndStatus()
    {
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            context.LoanRequests.Add(new LoanRequest(1, 1, 1, DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "P1"));
            context.LoanRequests.Add(new LoanRequest(2, 2, 2, DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "P2"));
            await context.SaveChangesAsync();
        }

        using (var context = new LoanDbContext(options))
        {
            var repository = new LoanRequestRepository(context);
            
            var user1Reqs = await repository.GetAllAsync(userId: 1);
            Assert.Single(user1Reqs);
            Assert.Equal("P1", user1Reqs.First().Purpose);
        }
    }
}
