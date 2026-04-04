using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Entities;

/// <summary>
/// 備品エンティティ
/// </summary>
public class Equipment
{
    /// <summary>
    /// 備品ID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 備品名
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// カテゴリID
    /// </summary>
    public int CategoryId { get; private set; }
    /// <summary>
    /// 備品の状態
    /// </summary>
    public EquipmentStatus Status { get; private set; }
    /// <summary>
    /// 備品の説明
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">備品ID</param>
    /// <param name="name">備品名</param>
    /// <param name="categoryId">カテゴリID</param>
    /// <param name="status">備品の状態</param>
    /// <param name="description">備品の説明</param>
    public Equipment(int id, string name, int categoryId, EquipmentStatus status, string description)
    {
        Id = id;
        Name = name;
        CategoryId = categoryId;
        Status = status;
        Description = description;
    }

    /// <summary>
    /// 備品の状態を変更する
    /// </summary>
    /// <param name="newStatus">変更後の状態</param>
    /// <exception cref="DomainException"></exception>
    public void ChangeStatus(EquipmentStatus newStatus)
    {
        if (Status == EquipmentStatus.Disposed)
        {
            throw new DomainException("廃棄済の備品の状態は変更できません。");
        }

        if (Status == EquipmentStatus.UnderRepair && newStatus == EquipmentStatus.Loaned)
        {
            throw new DomainException("修理中の備品を直接貸出中に変更することはできません。");
        }

        Status = newStatus;
    }
}
