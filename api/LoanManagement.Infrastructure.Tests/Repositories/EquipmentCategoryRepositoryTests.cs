using LoanManagement.Domain.Entities;
using LoanManagement.Infrastructure.Persistence;
using LoanManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LoanManagement.Infrastructure.Tests.Repositories;

public class EquipmentCategoryRepositoryTests
{
    private DbContextOptions<LoanDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<LoanDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task AddAsync_ShouldAddCategory()
    {
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            var repository = new EquipmentCategoryRepository(context);
            var category = new EquipmentCategory(1, "Notebook PC", "Laptops");
            await repository.AddAsync(category);
        }

        using (var context = new LoanDbContext(options))
        {
            Assert.Equal(1, context.EquipmentCategories.Count());
            Assert.Equal("Notebook PC", context.EquipmentCategories.First().Name);
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            context.EquipmentCategories.Add(new EquipmentCategory(1, "PC", ""));
            context.EquipmentCategories.Add(new EquipmentCategory(2, "Monitor", ""));
            await context.SaveChangesAsync();
        }

        using (var context = new LoanDbContext(options))
        {
            var repository = new EquipmentCategoryRepository(context);
            var result = await repository.GetAllAsync();
            Assert.Equal(2, result.Count());
        }
    }
}
