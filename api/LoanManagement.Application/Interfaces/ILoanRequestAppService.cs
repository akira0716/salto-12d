using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Interfaces;

/// <summary>
/// 貸出申請管理に関するアプリケーションサービス（ユースケース）
/// </summary>
public interface ILoanRequestAppService
{
    /// <summary>
    /// 全ての貸出申請の一覧を取得（管理者向け）
    /// </summary>
    /// <returns>貸出申請情報の一覧</returns>
    Task<IEnumerable<LoanRequestDto>> GetAllAsync();

    /// <summary>
    /// 指定されたユーザーの貸出申請一覧を取得
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns>当該ユーザーの貸出申請情報の一覧</returns>
    Task<IEnumerable<LoanRequestDto>> GetMyRequestsAsync(int userId);

    /// <summary>
    /// 貸出ポリシーを検証した上で、新しい貸出申請を登録
    /// </summary>
    /// <param name="userId">申請するユーザーID</param>
    /// <param name="dto">申請内容</param>
    /// <returns>登録された申請のID</returns>
    Task<int> CreateAsync(int userId, LoanRequestCreateDto dto);

    /// <summary>
    /// 貸出申請を承認し、貸出処理を実行（管理者向け）
    /// </summary>
    /// <param name="requestId">申請ID</param>
    Task ApproveAsync(int requestId);

    /// <summary>
    /// 貸出申請を却下（管理者向け）
    /// </summary>
    /// <param name="requestId">申請ID</param>
    /// <param name="dto">却下理由等の情報</param>
    Task RejectAsync(int requestId, LoanRequestRejectDto dto);
}
