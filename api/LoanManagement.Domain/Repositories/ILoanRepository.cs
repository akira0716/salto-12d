using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Repositories;

public interface ILoanRepository
{
    /// <summary>
    /// 貸出の個別取得
    /// </summary>
    /// <param name="id">貸出ID</param>
    /// <returns></returns>
    Task<Loan?> GetByIdAsync(int id);
    /// <summary>
    /// 特定ユーザーの「貸出中」の貸出の取得（ポリシー検証用）
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    Task<IEnumerable<Loan>> GetActiveLoansByUserIdAsync(int userId);
    /// <summary>
    /// 全貸出の取得
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Loan>> GetAllAsync();
    /// <summary>
    /// 貸出の新規登録
    /// </summary>
    /// <param name="loan">登録する貸出</param>
    /// <returns></returns>
    Task AddAsync(Loan loan);
    /// <summary>
    /// 返却処理時の更新
    /// </summary>
    /// <param name="loan">更新する貸出</param>
    /// <returns></returns>
    Task UpdateAsync(Loan loan);
}
