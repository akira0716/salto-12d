using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Repositories;
using LoanManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class EquipmentCategoryRepository : IEquipmentCategoryRepository
{
    private readonly LoanDbContext _context;

    public EquipmentCategoryRepository(LoanDbContext context)
    {
        _context = context;
    }

    #region 全カテゴリの取得

    /// <summary>
    /// 全カテゴリの取得
    /// </summary>
    /// <returns>全カテゴリ</returns>
    public async Task<IEnumerable<EquipmentCategory>> GetAllAsync()
    {
        return await _context.EquipmentCategories.ToListAsync();
    }

    #endregion

    #region 単一カテゴリの取得

    /// <summary>
    /// 単一カテゴリの取得
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <returns>カテゴリ</returns>
    public async Task<EquipmentCategory?> GetByIdAsync(int id)
    {
        return await _context.EquipmentCategories.FindAsync(id);
    }

    #endregion

    #region カテゴリの新規登録

    /// <summary>
    /// カテゴリの新規登録
    /// </summary>
    /// <param name="category">カテゴリ</param>
    /// <returns></returns>
    public async Task AddAsync(EquipmentCategory category)
    {
        _context.EquipmentCategories.Add(category);
        await _context.SaveChangesAsync();
    }

    #endregion
}
