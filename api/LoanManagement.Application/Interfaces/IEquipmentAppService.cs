using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.Interfaces;

/// <summary>
/// 備品管理に関するアプリケーションサービス（ユースケース）
/// </summary>
public interface IEquipmentAppService
{
    /// <summary>
    /// 指定されたIDの備品情報を取得
    /// </summary>
    /// <param name="id">備品ID</param>
    /// <returns>備品情報（見つからない場合はnull）</returns>
    Task<EquipmentDto?> GetByIdAsync(int id);

    /// <summary>
    /// 条件に合致する備品一覧を取得
    /// </summary>
    /// <param name="categoryId">カテゴリID（オプション）</param>
    /// <param name="keyword">検索キーワード（オプション）</param>
    /// <param name="status">備品ステータス（オプション）</param>
    /// <returns>備品情報の一覧</returns>
    Task<IEnumerable<EquipmentDto>> GetAllAsync(int? categoryId = null, string? keyword = null, EquipmentStatus? status = null);

    /// <summary>
    /// 新しい備品を登録
    /// </summary>
    /// <param name="dto">登録する備品情報</param>
    /// <returns>登録された備品のID</returns>
    Task<int> CreateAsync(EquipmentCreateDto dto);

    /// <summary>
    /// 備品情報を更新
    /// </summary>
    /// <param name="id">備品ID</param>
    /// <param name="dto">更新する備品情報</param>
    Task UpdateAsync(int id, EquipmentUpdateDto dto);
}
