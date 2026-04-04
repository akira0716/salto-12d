namespace LoanManagement.Domain.Entities;

/// <summary>
/// 備品カテゴリーエンティティ
/// </summary>
public class EquipmentCategory
{
    /// <summary>
    /// カテゴリーID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// カテゴリー名
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// カテゴリー説明
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">カテゴリーID</param>
    /// <param name="name">カテゴリー名</param>
    /// <param name="description">カテゴリー説明</param>
    public EquipmentCategory(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
