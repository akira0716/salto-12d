using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Repositories;

public interface IEquipmentRepository
{
    /// <summary>
    /// 備品の個別取得
    /// </summary>
    /// <param name="id">備品ID</param>
    /// <returns></returns>
    Task<Equipment?> GetByIdAsync(int id);
    /// <summary>
    /// 備品一覧取得
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Equipment>> GetAllAsync();
    /// <summary>
    /// 備品の新規登録
    /// </summary>
    /// <param name="equipment">登録する備品</param>
    /// <returns></returns>
    Task AddAsync(Equipment equipment);
    /// <summary>
    /// 備品の状態更新
    /// </summary>
    /// <param name="equipment">更新する備品</param>
    /// <returns></returns>
    Task UpdateAsync(Equipment equipment);
}
