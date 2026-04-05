using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.DTOs;

/// <summary>
/// ユーザー情報の参照用データ転送オブジェクト
/// </summary>
public class UserDto
{
    /// <summary>
    /// ユーザーID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// ユーザー名
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// メールアドレス
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// 権限
    /// </summary>
    public Role Role { get; set; }
}
