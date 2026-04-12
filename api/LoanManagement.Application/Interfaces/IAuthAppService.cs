using LoanManagement.Application.DTOs.Auth;

namespace LoanManagement.Application.Interfaces;

/// <summary>
/// 認証アプリケーションサービス
/// </summary>
public interface IAuthAppService
{
    /// <summary>
    /// ログイン処理
    /// </summary>
    /// <param name="request"></param>
    /// <returns>ログイン成功時はトークンを含む結果、失敗時はnullを返す</returns>
    Task<LoginResultDto?> LoginAsync(LoginRequestDto request);

    /// <summary>
    /// サインアップ処理
    /// </summary>
    /// <param name="request"></param>
    /// <returns>新規登録後に生成されたトークンを含む結果、既にメールが使用されている場合はnull等をハンドリング</returns>
    Task<LoginResultDto?> SignupAsync(SignupRequestDto request);
}
