namespace LoanManagement.Domain.Enums;

/// <summary>
/// 備品の状態を表す列挙型
/// </summary>
public enum EquipmentStatus
{
    /// <summary>
    /// 利用可能
    /// </summary>
    Available = 1,
    /// <summary>
    /// 貸出中
    /// </summary>
    Loaned = 2,
    /// <summary>
    /// 修理中
    /// </summary>
    UnderRepair = 3,
    /// <summary>
    /// 廃棄済
    /// </summary>
    Disposed = 4
}
