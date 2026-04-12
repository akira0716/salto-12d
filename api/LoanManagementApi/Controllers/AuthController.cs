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

    /// <summary>
    /// サインアップAPI（ユーザー登録）
    /// </summary>
    /// <param name="request">サインアップ情報</param>
    /// <returns></returns>
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequestDto request)
    {
        try
        {
            var result = await _authAppService.SignupAsync(request);
            if (result == null)
            {
                return BadRequest(new { message = "サインアップに失敗しました。" });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
