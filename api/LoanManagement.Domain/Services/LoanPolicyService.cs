using System;
using System.Collections.Generic;
using System.Linq;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Services;

/// <summary>
/// 貸出に関するビジネスルールを検証するサービスクラス
/// </summary>
public class LoanPolicyService
{
    /// <summary>
    /// 貸出数と申請数の合計、および貸出期間の最大日数
    /// </summary>
    private const int MaxActiveLoansAndRequests = 3;
    /// <summary>
    /// 1回の貸出期間の最大日数
    /// </summary>
    private const int MaxLoanDurationDays = 30;

    /// <summary>
    /// 新規の貸出申請が可能かどうかを検証します。
    /// 条件を満たさない場合は <see cref="DomainException"/> がスローされます。
    /// </summary>
    /// <param name="user">ユーザー</param>
    /// <param name="userPendingRequests">ユーザーの保留中の貸出申請</param>
    /// <param name="userActiveLoans">ユーザーの現在の貸出</param>
    /// <param name="currentDate">現在の日付</param>
    /// <param name="startDate">貸出開始日</param>
    /// <param name="endDate">貸出終了日</param>
    /// <exception cref="DomainException"></exception>
    public void ValidateLoanRequest(User user, IEnumerable<LoanRequest> userPendingRequests, IEnumerable<Loan> userActiveLoans, DateTime currentDate, DateTime startDate, DateTime endDate)
    {
        // ルール1: 1回の貸出期限は最大30日まで
        var duration = (endDate - startDate).Days;
        if (duration > MaxLoanDurationDays || duration <= 0)
        {
            throw new DomainException($"貸出期間は1日以上、最大{MaxLoanDurationDays}日までです。");
        }

        // ルール2: 利用者は遅延している貸出が1つでもある場合、新規申請ができない
        var hasOverdueLoans = userActiveLoans.Any(l => l.IsOverdue(currentDate));
        if (hasOverdueLoans)
        {
            throw new DomainException("遅延している貸出があるため、新規の申請を行うことはできません。");
        }

        // ルール3: 利用者が現在保持している「申請中」の貸出申請と「貸出中」の貸出の合計数は、最大3件を超えてはならない
        var currentTotal = userPendingRequests.Count(r => r.Status == LoanRequestStatus.Pending) +
                           userActiveLoans.Count(l => l.Status == LoanStatus.Active);

        if (currentTotal >= MaxActiveLoansAndRequests)
        {
            throw new DomainException($"「申請中」および「貸出中」の合計が最大{MaxActiveLoansAndRequests}件に達しているため、これ以上の申請はできません。");
        }
    }
}
