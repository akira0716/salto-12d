namespace LoanManagement.Application.DTOs.Auth;

/// <summary>
/// ログイン結果
/// </summary>
public class LoginResultDto
{
    public string Token { get; set; }

    public LoginResultDto(string token)
    {
        Token = token;
    }
}
