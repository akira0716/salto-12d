using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers;

/// <summary>
/// 設備管理APIコントローラー
/// </summary>
[ApiController]
[Route("api/v1/equipments")]
public class EquipmentsController : ControllerBase
{
    private readonly IEquipmentAppService _equipmentAppService;

    public EquipmentsController(IEquipmentAppService equipmentAppService)
    {
        _equipmentAppService = equipmentAppService;
    }

    /// <summary>
    /// 全ての設備を取得する
    /// </summary>
    /// <param name="category_id">カテゴリID</param>
    /// <param name="keyword">検索キーワード</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAll(
        [FromQuery] int? category_id,
        [FromQuery] string? keyword,
        [FromQuery] string? status)
    {
        EquipmentStatus? equipmentStatus = null;
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<EquipmentStatus>(status, true, out var parsedStatus))
        {
            equipmentStatus = parsedStatus;
        }

        var isEmployee = User.IsInRole("Employee");
        if (isEmployee)
        {
            equipmentStatus = EquipmentStatus.Available;
        }

        var results = await _equipmentAppService.GetAllAsync(category_id, keyword, equipmentStatus);

        // API定義に基づくJSON構造へマッピング
        return Ok(new { equipments = results });
    }

    /// <summary>
    /// IDで設備を取得する
    /// </summary>
    /// <param name="id">設備ID</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<EquipmentDto>> GetById([FromRoute] int id)
    {
        // 指定されたIDの備品情報を取得
        var equipment = await _equipmentAppService.GetByIdAsync(id);
        if (equipment == null) return NotFound();

        return Ok(equipment);
    }

    /// <summary>
    /// 新しい設備を作成する
    /// </summary>
    /// <param name="dto">作成する設備の情報</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] EquipmentCreateDto dto)
    {
        // 新しい備品を登録
        var id = await _equipmentAppService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>
    /// 既存の設備を更新する
    /// </summary>
    /// <param name="id">設備ID</param>
    /// <param name="dto">更新する設備の情報</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] EquipmentUpdateDto dto)
    {
        try
        {
            //  備品情報を更新
            await _equipmentAppService.UpdateAsync(id, dto);
            return NoContent();
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
