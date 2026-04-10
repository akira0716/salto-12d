using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Repositories;
using LoanManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly LoanDbContext _context;

    public EquipmentRepository(LoanDbContext context)
    {
        _context = context;
    }

    #region 備品の新規登録

    /// <summary>
    /// 備品の新規登録
    /// </summary>
    /// <param name="equipment">備品</param>
    /// <returns></returns>
    public async Task AddAsync(Equipment equipment)
    {
        _context.Equipments.Add(equipment);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region 備品の一覧取得（カテゴリ、キーワード、ステータスでフィルタリング可能）

    /// <summary>
    /// 備品の一覧取得（カテゴリ、キーワード、ステータスでフィルタリング可能）
    /// </summary>
    /// <param name="categoryId">カテゴリID</param>
    /// <param name="keyword">キーワード</param>
    /// <param name="status">ステータス</param>
    /// <returns>備品一覧</returns>
    public async Task<IEnumerable<Equipment>> GetAllAsync(int? categoryId = null, string? keyword = null, EquipmentStatus? status = null)
    {
        var query = _context.Equipments
            .Include(e => e.Category)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(e => e.Name.Contains(keyword) || (e.Description != null && e.Description.Contains(keyword)));
        }

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        return await query.ToListAsync();
    }

    #endregion

    #region 備品の個別取得

    /// <summary>
    /// 備品の個別取得
    /// </summary>
    /// <param name="id">備品ID</param>
    /// <returns>備品</returns>
    public async Task<Equipment?> GetByIdAsync(int id)
    {
        return await _context.Equipments
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    #endregion

    #region 備品の状態更新

    /// <summary>
    /// 備品の状態更新
    /// </summary>
    /// <param name="equipment">備品</param>
    /// <returns></returns>
    public async Task UpdateAsync(Equipment equipment)
    {
        _context.Equipments.Update(equipment);
        await _context.SaveChangesAsync();
    }

    #endregion
}
