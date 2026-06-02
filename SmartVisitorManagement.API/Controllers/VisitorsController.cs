using Microsoft.AspNetCore.Mvc;
using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.DTOs.Visitor;
using SmartVisitorManagement.Core.Filters;
using SmartVisitorManagement.Core.Interfaces.Services;

namespace SmartVisitorManagement.API.Controllers;

[ApiController]
[Route("api/visitors")]
[Produces("application/json")]
public class VisitorsController : ControllerBase
{
    private readonly IVisitorService _visitorService;

    public VisitorsController(IVisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    /// <summary>Get all visitors with optional search and pagination.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VisitorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] VisitorSearchFilter filter)
    {
        var result = await _visitorService.GetAllAsync(filter);
        return Ok(ApiResponse<PagedResult<VisitorResponseDto>>.SuccessResponse(result));
    }

    /// <summary>Get a visitor by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<VisitorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var visitor = await _visitorService.GetByIdAsync(id);
        if (visitor is null)
            return NotFound(ApiResponse<VisitorResponseDto>.FailResponse($"Visitor with ID {id} not found."));

        return Ok(ApiResponse<VisitorResponseDto>.SuccessResponse(visitor));
    }

    /// <summary>Register a new visitor.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VisitorResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVisitorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<VisitorResponseDto>.FailResponse(
                "Validation failed.",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            ));

        var created = await _visitorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id },
            ApiResponse<VisitorResponseDto>.SuccessResponse(created, "Visitor registered successfully."));
    }

    /// <summary>Update visitor information.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<VisitorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVisitorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<VisitorResponseDto>.FailResponse(
                "Validation failed.",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            ));

        var updated = await _visitorService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound(ApiResponse<VisitorResponseDto>.FailResponse($"Visitor with ID {id} not found."));

        return Ok(ApiResponse<VisitorResponseDto>.SuccessResponse(updated, "Visitor updated successfully."));
    }

    /// <summary>Delete a visitor.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _visitorService.DeleteAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<object>.FailResponse($"Visitor with ID {id} not found."));

        return NoContent();
    }
}