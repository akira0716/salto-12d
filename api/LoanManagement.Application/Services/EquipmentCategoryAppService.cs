using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Repositories;

namespace LoanManagement.Application.Services;

/// <summary>
/// 備品カテゴリ管理に関するアプリケーションサービスの実装
/// </summary>
public class EquipmentCategoryAppService : IEquipmentCategoryAppService
{
    private readonly IEquipmentCategoryRepository _categoryRepository;

    public EquipmentCategoryAppService(IEquipmentCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    #region 全ての備品カテゴリ一覧を取得

    /// <summary>
    /// 全ての備品カテゴリ一覧を取得
    /// </summary>
    /// <returns>備品カテゴリ情報の一覧</returns>
    public async Task<IEnumerable<EquipmentCategoryDto>> GetAllAsync()
    {
        // 備品カテゴリー全件取得
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => new EquipmentCategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
    }

    #endregion

    #region 新しい備品カテゴリを登録

    /// <summary>
    /// 新しい備品カテゴリを登録
    /// </summary>
    /// <param name="dto">登録する備品カテゴリ情報</param>
    /// <returns>登録されたカテゴリのID</returns>
    public async Task<int> CreateAsync(EquipmentCategoryCreateDto dto)
    {
        // ドメインエンティティに変換してリポジトリに保存（IDはリポジトリ側で生成）
        var category = new EquipmentCategory(0, dto.Name, dto.Description);
        await _categoryRepository.AddAsync(category);
        return category.Id;
    }

    #endregion
}
