using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Repositories;

public interface ILoanRequestRepository
{
    /// <summary>
    /// 申請の個別取得
    /// </summary>
    /// <param name="id">申請ID</param>
    /// <returns></returns>
    Task<LoanRequest?> GetByIdAsync(int id);
    /// <summary>
    /// 特定ユーザーの「申請中」の申請の取得（ポリシー検証用）
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    Task<IEnumerable<LoanRequest>> GetPendingRequestsByUserIdAsync(int userId);
    /// <summary>
    /// 申請一覧の取得（管理者用）
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<LoanRequest>> GetAllAsync();
    /// <summary>
    /// 申請の新規登録
    /// </summary>
    /// <param name="request">登録する申請</param>
    /// <returns></returns>
    Task AddAsync(LoanRequest request);
    /// <summary>
    /// 承認・却下時の更新
    /// </summary>
    /// <param name="request">更新する申請</param>
    /// <returns></returns>
    Task UpdateAsync(LoanRequest request);
}
