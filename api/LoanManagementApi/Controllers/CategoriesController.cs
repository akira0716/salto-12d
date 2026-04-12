using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers;

/// <summary>
/// 設備カテゴリを管理するコントローラー
/// </summary>
[ApiController]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IEquipmentCategoryAppService _categoryAppService;

    public CategoriesController(IEquipmentCategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    /// <summary>
    /// 全ての設備カテゴリを取得する
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<EquipmentCategoryDto>>> GetAll()
    {
        // 全ての備品カテゴリ一覧を取得
        var results = await _categoryAppService.GetAllAsync();
        return Ok(results);
    }

    /// <summary>
    /// 設備カテゴリを作成する
    /// </summary>
    /// <param name="dto">作成する設備カテゴリの情報</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] EquipmentCategoryCreateDto dto)
    {
        // 新しい備品カテゴリを登録
        var id = await _categoryAppService.CreateAsync(dto);
        // GETするエンドポイントが設計上は無いですが、201 Created を返します
        return Created("", new { id });
    }
}
