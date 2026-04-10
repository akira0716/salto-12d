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

public class EquipmentRepositoryTests
{
    private DbContextOptions<LoanDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<LoanDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllAsync_ShouldFilterCorrectly()
    {
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            context.EquipmentCategories.Add(new EquipmentCategory(1, "PC", ""));
            context.EquipmentCategories.Add(new EquipmentCategory(2, "Monitor", ""));
            context.Equipments.Add(new Equipment(1, "MacBook", 1, EquipmentStatus.Available, "Apple"));
            context.Equipments.Add(new Equipment(2, "Dell XPS", 1, EquipmentStatus.Loaned, "Dell"));
            context.Equipments.Add(new Equipment(3, "LG Monitor", 2, EquipmentStatus.Available, "LG"));
            await context.SaveChangesAsync();
        }

        using (var context = new LoanDbContext(options))
        {
            var repository = new EquipmentRepository(context);

            // カテゴリIDで絞り込み
            var pcs = await repository.GetAllAsync(categoryId: 1);
            Assert.Equal(2, pcs.Count());

            // ステータスで絞り込み
            var loaned = await repository.GetAllAsync(status: EquipmentStatus.Loaned);
            Assert.Single(loaned);
            Assert.Equal("Dell XPS", loaned.First().Name);

            // キーワードで絞り込み
            var keywordMatch = await repository.GetAllAsync(keyword: "Apple");
            Assert.Single(keywordMatch);
            Assert.Equal("MacBook", keywordMatch.First().Name);
            
            // カテゴリ情報が含まれているか
            Assert.NotNull(keywordMatch.First().Category);
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEquipment()
    {
        var options = GetDbContextOptions();
        using (var context = new LoanDbContext(options))
        {
            context.EquipmentCategories.Add(new EquipmentCategory(1, "PC", ""));
            context.Equipments.Add(new Equipment(1, "MacBook", 1, EquipmentStatus.Available, ""));
            await context.SaveChangesAsync();
        }

        using (var context = new LoanDbContext(options))
        {
            var repository = new EquipmentRepository(context);
            var equipment = await repository.GetByIdAsync(1);
            equipment!.ChangeStatus(EquipmentStatus.Loaned);
            await repository.UpdateAsync(equipment);
        }

        using (var context = new LoanDbContext(options))
        {
            var updated = await context.Equipments.FindAsync(1);
            Assert.Equal(EquipmentStatus.Loaned, updated!.Status);
        }
    }
}
