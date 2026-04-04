using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Entities;

/// <summary>
/// 貸出エンティティ
/// </summary>
public class Loan
{
    /// <summary>
    /// 貸出ID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 貸出申請ID
    /// </summary>
    public int LoanRequestId { get; private set; }
    /// <summary>
    /// ユーザーID
    /// </summary>
    public int UserId { get; private set; }
    /// <summary>
    /// 備品ID
    /// </summary>
    public int EquipmentId { get; private set; }
    /// <summary>
    /// 貸出日
    /// </summary>
    public DateTime LoanDate { get; private set; }
    /// <summary>
    /// 返却予定日
    /// </summary>
    public DateTime DueDate { get; private set; }
    /// <summary>
    /// 返却日
    /// </summary>
    public DateTime? ReturnDate { get; private set; }
    /// <summary>
    /// 貸出状態
    /// </summary>
    public LoanStatus Status { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">貸出ID</param>
    /// <param name="loanRequestId">貸出申請ID</param>
    /// <param name="userId">ユーザーID</param>
    /// <param name="equipmentId">備品ID</param>
    /// <param name="loanDate">貸出日</param>
    /// <param name="dueDate">返却予定日</param>
    /// <exception cref="DomainException"></exception>
    public Loan(int id, int loanRequestId, int userId, int equipmentId, DateTime loanDate, DateTime dueDate)
    {
        if (dueDate <= loanDate)
        {
            throw new DomainException("返却予定日は貸出日より後の日付でなければなりません。");
        }

        Id = id;
        LoanRequestId = loanRequestId;
        UserId = userId;
        EquipmentId = equipmentId;
        LoanDate = loanDate;
        DueDate = dueDate;
        Status = LoanStatus.Active;
    }

    /// <summary>
    /// 返却処理
    /// </summary>
    /// <param name="returnDate">返却日</param>
    /// <exception cref="DomainException"></exception>
    public void Return(DateTime returnDate)
    {
        if (Status != LoanStatus.Active)
        {
            throw new DomainException("貸出中ではないため返却できません。");
        }

        Status = LoanStatus.Returned;
        ReturnDate = returnDate;
    }

    /// <summary>
    /// 貸出が期限切れかどうかを判断する
    /// </summary>
    /// <param name="currentDate"></param>
    /// <returns></returns>
    public bool IsOverdue(DateTime currentDate)
    {
        return Status == LoanStatus.Active && currentDate > DueDate;
    }
}
