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

public class LoanRepositoryTests
{
    private DbContextOptions<LoanDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<LoanDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetActiveLoansByUserIdAsync_ShouldReturnOnlyActive()
    {
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            var loan1 = new Loan(1, 1, 1, 1, DateTime.Now, DateTime.Now.AddDays(2));
            var loan2 = new Loan(2, 2, 1, 2, DateTime.Now, DateTime.Now.AddDays(2));
            loan2.Return(DateTime.Now);

            context.Loans.Add(loan1);
            context.Loans.Add(loan2);
            await context.SaveChangesAsync();
        }

        using (var context = new LoanDbContext(options))
        {
            var repository = new LoanRepository(context);
            var result = await repository.GetActiveLoansByUserIdAsync(1);

            Assert.Single(result);
            Assert.Equal(LoanStatus.Active, result.First().Status);
            Assert.Equal(1, result.First().Id);
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateLoan()
    {
        var options = GetDbContextOptions();
        var loanDate = DateTime.Now;
        using (var context = new LoanDbContext(options))
        {
            context.Loans.Add(new Loan(1, 1, 1, 1, loanDate, loanDate.AddDays(2)));
            await context.SaveChangesAsync();
        }

        var returnDate = loanDate.AddDays(1);
        using (var context = new LoanDbContext(options))
        {
            var repository = new LoanRepository(context);
            var loan = await repository.GetByIdAsync(1);
            loan!.Return(returnDate);
            await repository.UpdateAsync(loan);
        }

        using (var context = new LoanDbContext(options))
        {
            var updated = await context.Loans.FindAsync(1);
            Assert.Equal(LoanStatus.Returned, updated!.Status);
            Assert.Equal(returnDate, updated.ReturnDate);
        }
    }
}
