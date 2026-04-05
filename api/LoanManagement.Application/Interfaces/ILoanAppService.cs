using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Interfaces;

/// <summary>
/// 貸出状況管理および返却に関するアプリケーションサービス（ユースケース）
/// </summary>
public interface ILoanAppService
{
    /// <summary>
    /// 全ての貸出情報一覧を取得（管理者向け）
    /// </summary>
    /// <returns>貸出状況情報の一覧</returns>
    Task<IEnumerable<LoanDto>> GetAllAsync();

    /// <summary>
    /// 指定されたユーザーの現在有効な「貸出中」情報一覧を取得
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns>当該ユーザーの現在貸出中の情報一覧</returns>
    Task<IEnumerable<LoanDto>> GetActiveByUserIdAsync(int userId);

    /// <summary>
    /// 備品の返却処理（管理者向け）
    /// </summary>
    /// <param name="loanId">貸出ID</param>
    Task ReturnEquipmentAsync(int loanId);
}
