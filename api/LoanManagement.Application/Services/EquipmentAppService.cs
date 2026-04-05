using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Repositories;

namespace LoanManagement.Application.Services;

/// <summary>
/// 備品管理に関するアプリケーションサービスの実装
/// </summary>
public class EquipmentAppService : IEquipmentAppService
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IEquipmentCategoryRepository _categoryRepository;

    public EquipmentAppService(IEquipmentRepository equipmentRepository, IEquipmentCategoryRepository categoryRepository)
    {
        _equipmentRepository = equipmentRepository;
        _categoryRepository = categoryRepository;
    }

    #region 指定されたIDの備品情報を取得

    /// <summary>
    /// 指定されたIDの備品情報を取得
    /// </summary>
    /// <param name="id">備品ID</param>
    /// <returns>備品情報（見つからない場合はnull）</returns>
    public async Task<EquipmentDto?> GetByIdAsync(int id)
    {
        // 備品をIDで取得（カテゴリ情報が結合されている前提）
        var eq = await _equipmentRepository.GetByIdAsync(id);
        if (eq == null) return null;

        return new EquipmentDto
        {
            Id = eq.Id,
            Name = eq.Name,
            CategoryId = eq.CategoryId,
            CategoryName = eq.Category?.Name ?? "不明",
            Status = eq.Status,
            Description = eq.Description
        };
    }

    #endregion

    #region 条件に合致する備品一覧を取得

    /// <summary>
    /// 条件に合致する備品一覧を取得
    /// </summary>
    /// <param name="categoryId">カテゴリID（オプション）</param>
    /// <param name="keyword">検索キーワード（オプション）</param>
    /// <param name="status">備品ステータス（オプション）</param>
    /// <returns>備品情報の一覧</returns>
    public async Task<IEnumerable<EquipmentDto>> GetAllAsync(int? categoryId = null, string? keyword = null, EquipmentStatus? status = null)
    {
        // 備品を全件または条件付きで取得（カテゴリ情報が結合されている前提）
        var equipments = await _equipmentRepository.GetAllAsync(categoryId, keyword, status);

        var dtoList = new List<EquipmentDto>();
        foreach (var eq in equipments)
        {
            dtoList.Add(new EquipmentDto
            {
                Id = eq.Id,
                Name = eq.Name,
                CategoryId = eq.CategoryId,
                CategoryName = eq.Category?.Name ?? "不明",
                Status = eq.Status,
                Description = eq.Description
            });
        }

        return dtoList;
    }

    #endregion

    #region 新しい備品を登録

    /// <summary>
    /// 新しい備品を登録
    /// </summary>
    /// <param name="dto">登録する備品情報</param>
    /// <returns>登録された備品のID</returns>
    /// <exception cref="Exception"></exception>
    public async Task<int> CreateAsync(EquipmentCreateDto dto)
    {
        // カテゴリの存在確認
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null) throw new Exception("指定されたカテゴリが見つかりません。");

        // ドメインエンティティを作成（登録時はIDは0で、リポジトリが保存時に生成する）
        var equipment = new Equipment(0, dto.Name, dto.CategoryId, EquipmentStatus.Available, dto.Description);

        // ドメインエンティティをリポジトリに保存
        await _equipmentRepository.AddAsync(equipment);

        return equipment.Id;
    }

    #endregion

    #region 備品情報を更新

    /// <summary>
    /// 備品情報を更新
    /// </summary>
    /// <param name="id">備品ID</param>
    /// <param name="dto">更新する備品情報</param>
    public async Task UpdateAsync(int id, EquipmentUpdateDto dto)
    {
        // 備品をIDで取得
        var eq = await _equipmentRepository.GetByIdAsync(id);
        if (eq == null) throw new Exception("備品が見つかりません。");

        // ステータスの更新
        if (eq.Status != dto.Status)
        {
            eq.ChangeStatus(dto.Status);
        }

        // 基本情報の更新
        eq.UpdateDetails(dto.Name, dto.CategoryId, dto.Description);

        await _equipmentRepository.UpdateAsync(eq);
    }

    #endregion
}
