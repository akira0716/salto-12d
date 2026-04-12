using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Interfaces;

/// <summary>
/// 備品カテゴリ管理に関するアプリケーションサービス（ユースケース）
/// </summary>
public interface IEquipmentCategoryAppService
{
    /// <summary>
    /// 全ての備品カテゴリ一覧を取得
    /// </summary>
    /// <returns>備品カテゴリ情報の一覧</returns>
    Task<IEnumerable<EquipmentCategoryDto>> GetAllAsync();

    /// <summary>
    /// 新しい備品カテゴリを登録
    /// </summary>
    /// <param name="dto">登録する備品カテゴリ情報</param>
    Task<int> CreateAsync(EquipmentCategoryCreateDto dto);

    /// <summary>
    /// カテゴリ情報を更新
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <param name="dto">更新する情報</param>
    Task UpdateAsync(int id, EquipmentCategoryDto dto);

    /// <summary>
    /// カテゴリを削除
    /// </summary>
    /// <param name="id">カテゴリID</param>
    Task DeleteAsync(int id);
}
