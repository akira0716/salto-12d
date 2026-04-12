using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers;

/// <summary>
/// 管理者向けのAPIコントローラー
/// </summary>
[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ILoanRequestAppService _loanRequestAppService;
    private readonly ILoanAppService _loanAppService;

    public AdminController(ILoanRequestAppService loanRequestAppService, ILoanAppService loanAppService)
    {
        _loanRequestAppService = loanRequestAppService;
        _loanAppService = loanAppService;
    }

    /// <summary>
    /// 貸出申請一覧の取得
    /// </summary>
    /// <returns></returns>
    [HttpGet("loan-requests")]
    public async Task<ActionResult<IEnumerable<LoanRequestDto>>> GetLoanRequests()
    {
        // 全ての貸出申請の一覧を取得（管理者向け）
        var results = await _loanRequestAppService.GetAllAsync();
        return Ok(results);
    }

    /// <summary>
    /// 全社の貸出状況の取得（遅延絞り込み含む）
    /// </summary>
    /// <returns></returns>
    [HttpGet("loans")]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoans()
    {
        // 全ての貸出情報一覧を取得（管理者向け）
        var results = await _loanAppService.GetAllAsync();
        return Ok(results);
    }

    /// <summary>
    /// 貸出申請の承認
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("loan-requests/{id}/approve")]
    public async Task<IActionResult> ApproveLoanRequest(int id)
    {
        try
        {
            // 貸出申請を承認し、貸出処理を実行（管理者向け）
            await _loanRequestAppService.ApproveAsync(id);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// 貸出申請の却下（却下理由の入力）
    /// </summary>
    /// <param name="id">貸出申請ID</param>
    /// <param name="dto">却下理由</param>
    /// <returns></returns>
    [HttpPatch("loan-requests/{id}/reject")]
    public async Task<IActionResult> RejectLoanRequest(int id, [FromBody] LoanRequestRejectDto dto)
    {
        try
        {
            // 貸出申請を却下（管理者向け）
            await _loanRequestAppService.RejectAsync(id, dto);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// 備品の返却受領処理
    /// </summary>
    /// <param name="id">貸出ID</param>
    /// <returns></returns>
    [HttpPatch("loans/{id}/return")]
    public async Task<IActionResult> ReturnEquipment(int id)
    {
        try
        {
            // 備品の返却処理（管理者向け）
            await _loanAppService.ReturnEquipmentAsync(id);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
