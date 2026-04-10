using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Repositories;
using LoanManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly LoanDbContext _context;

    public LoanRepository(LoanDbContext context)
    {
        _context = context;
    }

    #region 貸出の新規登録

    /// <summary>
    /// 貸出の新規登録
    /// </summary>
    /// <param name="loan">貸出</param>
    /// <returns></returns>
    public async Task AddAsync(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region ユーザーIDによる有効な貸出の取得

    /// <summary>
    /// ユーザーIDによる有効な貸出の取得
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns>有効な貸出一覧</returns>
    public async Task<IEnumerable<Loan>> GetActiveLoansByUserIdAsync(int userId)
    {
        return await _context.Loans
            .Where(l => l.UserId == userId && l.Status == LoanStatus.Active)
            .ToListAsync();
    }

    #endregion

    #region 全ての貸出の取得

    /// <summary>
    /// 全ての貸出の取得
    /// </summary>
    /// <returns>貸出一覧</returns>
    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _context.Loans
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();
    }

    #endregion

    #region 貸出の個別取得

    /// <summary>
    /// 貸出の個別取得
    /// </summary>
    /// <param name="id">貸出ID</param>
    /// <returns>貸出</returns>

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _context.Loans.FindAsync(id);
    }

    #endregion

    #region 返却処理時の更新

    /// <summary>
    /// 返却処理時の更新
    /// </summary>
    /// <param name="loan">貸出</param>
    /// <returns></returns>
    public async Task UpdateAsync(Loan loan)
    {
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
    }

    #endregion
}
