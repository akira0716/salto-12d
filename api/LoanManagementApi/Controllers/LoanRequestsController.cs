using System.Security.Claims;
using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers;

/// <summary>
/// 貸出申請APIコントローラー
/// </summary>
[ApiController]
[Route("api/v1/loan-requests")]
public class LoanRequestsController : ControllerBase
{
    private readonly ILoanRequestAppService _loanRequestAppService;

    public LoanRequestsController(ILoanRequestAppService loanRequestAppService)
    {
        _loanRequestAppService = loanRequestAppService;
    }

    /// <summary>
    /// 新しい貸出申請を作成する
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] LoanRequestCreateDto dto)
    {
        // 認証されたユーザーのIDを取得
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized(new { message = "無効なユーザーIDです" });
        }

        try
        {
            // 新しい貸出申請を登録
            var id = await _loanRequestAppService.CreateAsync(userId, dto);
            return StatusCode(StatusCodes.Status201Created, new { id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<LoanRequestDto>>> GetMyRequests()
    {
        // 認証されたユーザーのIDを取得
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized(new { message = "無効なユーザーIDです" });
        }

        // 指定されたユーザーの貸出申請一覧を取得
        var results = await _loanRequestAppService.GetMyRequestsAsync(userId);
        return Ok(results);
    }
}
