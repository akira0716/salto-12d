using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Repositories;

public interface IEquipmentCategoryRepository
{
    /// <summary>
    /// 単一カテゴリの取得
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <returns></returns>
    Task<EquipmentCategory?> GetByIdAsync(int id);
    /// <summary>
    /// 全カテゴリの取得
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<EquipmentCategory>> GetAllAsync();
}
