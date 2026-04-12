using LoanManagement.Application.DTOs.Auth;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers;

/// <summary>
/// 認証関連のAPIコントローラー
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthAppService _authAppService;

    public AuthController(IAuthAppService authAppService)
    {
        _authAppService = authAppService;
    }

    /// <summary>
    /// ログインAPI
    /// </summary>
    /// <param name="request">ログイン情報</param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        // ログイン処理を実行
        var result = await _authAppService.LoginAsync(request);
        if (result == null)
        {
            return Unauthorized(new { message = "メールアドレスまたはパスワードが正しくありません。" });
        }

        return Ok(result);
    }
}
