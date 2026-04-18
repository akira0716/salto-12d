using System.Security.Claims;
using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers;

/// <summary>
/// 貸出APIコントローラー
/// </summary>
[ApiController]
[Route("api/v1/loans")]
public class LoansController : ControllerBase
{
    private readonly ILoanAppService _loanAppService;

    public LoansController(ILoanAppService loanAppService)
    {
        _loanAppService = loanAppService;
    }

    /// <summary>
    /// 現在ログインしているユーザーの有効な貸出（現在貸出中）の一覧を取得する
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetMyLoans()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized(new { message = "無効なユーザーIDです" });
        }

        var results = await _loanAppService.GetActiveByUserIdAsync(userId);
        return Ok(new { loans = results });
    }
}
