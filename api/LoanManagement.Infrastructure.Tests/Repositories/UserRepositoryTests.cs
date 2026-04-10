using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Infrastructure.Persistence;
using LoanManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LoanManagement.Infrastructure.Tests.Repositories;

public class UserRepositoryTests
{
    private DbContextOptions<LoanDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<LoanDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            context.Users.Add(new User(1, "Test User", "test@example.com", "hash", Role.Employee));
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new LoanDbContext(options))
        {
            var repository = new UserRepository(context);
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
        }
    }
}
