using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.DTOs;

/// <summary>
/// 備品情報の参照用データ転送オブジェクト
/// </summary>
public class EquipmentDto
{
    /// <summary>
    /// 備品ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 備品名
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// カテゴリID
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// カテゴリ名
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;
    /// <summary>
    /// 備品の状態
    /// </summary>
    public EquipmentStatus Status { get; set; }
    /// <summary>
    /// 備品の説明
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// 備品の新規登録用データ転送オブジェクト
/// </summary>
public class EquipmentCreateDto
{
    /// <summary>
    /// 備品名
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// カテゴリID
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// 備品の説明
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// 備品情報の更新用データ転送オブジェクト
/// </summary>
public class EquipmentUpdateDto
{
    /// <summary>
    /// 備品名
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// カテゴリID
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// 備品の状態
    /// </summary>
    public EquipmentStatus Status { get; set; }
    /// <summary>
    /// 備品の説明
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
