using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.DTOs;

/// <summary>
/// 貸出申請情報の参照用データ転送オブジェクト
/// </summary>
public class LoanRequestDto
{
    /// <summary>
    /// 貸出申請ID
    /// </summary>
    public int Id { get; set; }
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
    /// 申請日時
    /// </summary>
    public DateTime RequestDate { get; set; }
    /// <summary>
    /// 貸出開始日時
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 貸出終了日時
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// 申請の状態
    /// </summary>
    public LoanRequestStatus Status { get; set; }
    /// <summary>
    /// 利用目的
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// 却下理由（申請が却下された場合にのみ設定される）
    /// </summary>
    public string RejectionReason { get; set; } = string.Empty;
}

/// <summary>
/// 貸出申請の新規登録用データ転送オブジェクト
/// </summary>
public class LoanRequestCreateDto
{
    /// <summary>
    /// 備品ID
    /// </summary>
    public int EquipmentId { get; set; }
    /// <summary>
    /// 貸出開始日時
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 貸出終了日時
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// 利用目的
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
}

/// <summary>
/// 貸出申請の却下処理用データ転送オブジェクト
/// </summary>
public class LoanRequestRejectDto
{
    /// <summary>
    /// 却下理由
    /// </summary>
    public string RejectionReason { get; set; } = string.Empty;
    /// <summary>
    /// 備品を故障状態に設定するかどうか
    /// </summary>
    public bool SetEquipmentBroken { get; set; }
}
