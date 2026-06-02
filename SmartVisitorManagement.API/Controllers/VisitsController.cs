using Microsoft.AspNetCore.Mvc;
using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.DTOs.Visit;
using SmartVisitorManagement.Core.Filters;
using SmartVisitorManagement.Core.Interfaces.Services;

namespace SmartVisitorManagement.API.Controllers;

[ApiController]
[Route("api/visits")]
[Produces("application/json")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitsController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    /// <summary>Check-in a visitor.</summary>
    [HttpPost("checkin")]
    [ProducesResponseType(typeof(ApiResponse<VisitResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<VisitResponseDto>.FailResponse(
                "Validation failed.",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            ));

        var visit = await _visitService.CheckInAsync(dto);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<VisitResponseDto>.SuccessResponse(visit, "Visitor checked in successfully."));
    }

    /// <summary>Check-out a visitor by visit ID.</summary>
    [HttpPut("checkout/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<VisitResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckOut(int id)
    {
        var visit = await _visitService.CheckOutAsync(id);
        if (visit is null)
            return NotFound(ApiResponse<VisitResponseDto>.FailResponse($"Visit with ID {id} not found."));

        return Ok(ApiResponse<VisitResponseDto>.SuccessResponse(visit, "Visitor checked out successfully."));
    }

    /// <summary>Get all currently active visitors.</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<List<VisitResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive()
    {
        var activeVisits = await _visitService.GetActiveVisitsAsync();
        return Ok(ApiResponse<List<VisitResponseDto>>.SuccessResponse(
            activeVisits, $"{activeVisits.Count} active visitor(s) found."));
    }

    /// <summary>Get paginated visit history with optional date filter.</summary>
    [HttpGet("history")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VisitResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory([FromQuery] VisitSearchFilter filter)
    {
        var result = await _visitService.GetHistoryAsync(filter);
        return Ok(ApiResponse<PagedResult<VisitResponseDto>>.SuccessResponse(result));
    }

    /// <summary>Get daily visit report. Defaults to today.</summary>
    [HttpGet("daily-report")]
    [ProducesResponseType(typeof(ApiResponse<DailyReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDailyReport([FromQuery] DateTime? date)
    {
        var reportDate = date ?? DateTime.UtcNow;
        var report = await _visitService.GetDailyReportAsync(reportDate);
        return Ok(ApiResponse<DailyReportDto>.SuccessResponse(report,
            $"Daily report for {reportDate:yyyy-MM-dd}."));
    }
}