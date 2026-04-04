namespace LoanManagement.Domain.Enums;

/// <summary>
/// 貸出申請の状態を表す列挙型
/// </summary>
public enum LoanRequestStatus
{
    /// <summary>
    /// 申請待ち
    /// </summary>
    Pending = 1,
    /// <summary>
    /// 承認済み
    /// </summary>
    Approved = 2,
    /// <summary>
    /// 却下
    /// </summary>
    Rejected = 3
}
