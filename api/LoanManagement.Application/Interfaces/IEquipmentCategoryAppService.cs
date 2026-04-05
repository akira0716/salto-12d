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
    /// <returns>登録されたカテゴリのID</returns>
    Task<int> CreateAsync(EquipmentCategoryCreateDto dto);
}
