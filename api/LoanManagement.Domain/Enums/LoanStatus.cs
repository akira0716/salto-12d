namespace LoanManagement.Domain.Enums;

/// <summary>
/// 貸出の状態を表す列挙型
/// </summary>
public enum LoanStatus
{
    /// <summary>
    /// 貸出中
    /// </summary>
    Active = 1,
    /// <summary>
    /// 返却済み
    /// </summary>
    Returned = 2
}
