namespace LoanManagement.Application.DTOs;

/// <summary>
/// 備品カテゴリ情報の参照用データ転送オブジェクト
/// </summary>
public class EquipmentCategoryDto
{
    /// <summary>
    /// 備品カテゴリID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 備品カテゴリ名
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 備品カテゴリの説明
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// 備品カテゴリの新規登録用データ転送オブジェクト
/// </summary>
public class EquipmentCategoryCreateDto
{
    /// <summary>
    /// 備品カテゴリ名
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 備品カテゴリの説明
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
