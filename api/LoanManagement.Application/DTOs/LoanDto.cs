using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.DTOs;

/// <summary>
/// 貸出情報の参照用データ転送オブジェクト
/// </summary>
public class LoanDto
{
    /// <summary>
    /// 貸出ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 貸出申請ID
    /// </summary>
    public int LoanRequestId { get; set; }
    /// <summary>
    /// ユーザーID
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// ユーザー名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 備品ID
    /// </summary>
    public int EquipmentId { get; set; }
    /// <summary>
    /// 備品名
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;
    /// <summary>
    /// 貸出開始日
    /// </summary>
    public DateTime LoanDate { get; set; }
    /// <summary>
    /// 返却予定日
    /// </summary>
    public DateTime DueDate { get; set; }
    /// <summary>
    /// 返却日
    /// </summary>
    public DateTime? ReturnDate { get; set; }
    /// <summary>
    /// 貸出状況
    /// </summary>
    public LoanStatus Status { get; set; }
    /// <summary>
    /// 期限超過フラグ
    /// </summary>
    public bool IsOverdue { get; set; }
}
