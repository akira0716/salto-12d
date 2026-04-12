using System.ComponentModel.DataAnnotations;

namespace LoanManagement.Application.DTOs.Auth;

/// <summary>
/// サインアップリクエスト
/// </summary>
public class SignupRequestDto
{
    /// <summary>
    /// ユーザー名
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// メールアドレス
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// パスワード
    /// </summary>
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// 確認用パスワード
    /// </summary>
    [Required]
    [Compare("Password", ErrorMessage = "パスワードと確認用パスワードが一致しません。")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
