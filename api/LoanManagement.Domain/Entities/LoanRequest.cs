using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Entities;

/// <summary>
/// 貸出申請エンティティ
/// </summary>
public class LoanRequest
{
    /// <summary>
    /// 貸出申請ID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// ユーザーID
    /// </summary>
    public int UserId { get; private set; }
    /// <summary>
    /// 備品ID
    /// </summary>
    public int EquipmentId { get; private set; }
    /// <summary>
    /// 申請日
    /// </summary>
    public DateTime RequestDate { get; private set; }
    /// <summary>
    /// 利用開始予定日
    /// </summary>
    public DateTime StartDate { get; private set; }
    /// <summary>
    /// 利用終了予定日
    /// </summary>
    public DateTime EndDate { get; private set; }
    /// <summary>
    /// ステータス
    /// </summary>
    public LoanRequestStatus Status { get; private set; }
    /// <summary>
    /// 利用目的
    /// </summary>
    public string Purpose { get; private set; }
    /// <summary>
    /// 却下理由
    /// </summary>
    public string RejectionReason { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">貸出申請ID</param>
    /// <param name="userId">ユーザーID</param>
    /// <param name="equipmentId">備品ID</param>
    /// <param name="requestDate">申請日</param>
    /// <param name="startDate">利用開始予定日</param>
    /// <param name="endDate">利用終了予定日</param>
    /// <param name="purpose">利用目的</param>
    /// <exception cref="DomainException"></exception>
    public LoanRequest(int id, int userId, int equipmentId, DateTime requestDate, DateTime startDate, DateTime endDate, string purpose)
    {
        if (endDate <= startDate)
        {
            throw new DomainException("利用終了希望日は利用開始希望日より後の日付でなければなりません。");
        }

        Id = id;
        UserId = userId;
        EquipmentId = equipmentId;
        RequestDate = requestDate;
        StartDate = startDate;
        EndDate = endDate;
        Status = LoanRequestStatus.Pending;
        Purpose = purpose;
        RejectionReason = string.Empty;
    }

    /// <summary>
    /// 申請を承認する
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void Approve()
    {
        if (Status != LoanRequestStatus.Pending)
        {
            throw new DomainException("申請中以外のステータスから承認することはできません。");
        }
        Status = LoanRequestStatus.Approved;
    }

    /// <summary>
    /// 申請を却下する
    /// </summary>
    /// <param name="reason">却下理由</param>
    /// <exception cref="DomainException"></exception>
    public void Reject(string reason)
    {
        if (Status != LoanRequestStatus.Pending)
        {
            throw new DomainException("申請中以外のステータスから却下することはできません。");
        }
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new DomainException("却下する場合は却下理由が必要です。");
        }

        Status = LoanRequestStatus.Rejected;
        RejectionReason = reason;
    }
}
