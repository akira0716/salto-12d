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
    
    /// <summary>
    /// カテゴリの新規登録
    /// </summary>
    /// <param name="category">カテゴリ情報</param>
    /// <returns></returns>
    Task AddAsync(EquipmentCategory category);

    /// <summary>
    /// カテゴリ情報の更新
    /// </summary>
    /// <param name="category">カテゴリ情報</param>
    /// <returns></returns>
    Task UpdateAsync(EquipmentCategory category);

    /// <summary>
    /// カテゴリの削除
    /// </summary>
    /// <param name="category">カテゴリ情報</param>
    /// <returns></returns>
    Task DeleteAsync(EquipmentCategory category);
}
