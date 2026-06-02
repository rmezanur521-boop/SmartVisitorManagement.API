using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.DTOs.Visit;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Core.Filters;
using SmartVisitorManagement.Core.Interfaces.Repositories;
using SmartVisitorManagement.Core.Interfaces.Services;

namespace SmartVisitorManagement.Services;

public class VisitService : IVisitService
{
    private readonly IVisitRepository _visitRepository;
    private readonly IVisitorRepository _visitorRepository;

    public VisitService(IVisitRepository visitRepository, IVisitorRepository visitorRepository)
    {
        _visitRepository = visitRepository;
        _visitorRepository = visitorRepository;
    }

    public async Task<VisitResponseDto> CheckInAsync(CheckInDto dto)
    {
        // Business rule: visitor must exist
        var visitor = await _visitorRepository.GetByIdAsync(dto.VisitorId);
        if (visitor is null)
            throw new KeyNotFoundException($"Visitor with ID {dto.VisitorId} not found.");

        // Business rule: visitor cannot check-in twice simultaneously
        var activeVisit = await _visitRepository.GetActiveVisitByVisitorIdAsync(dto.VisitorId);
        if (activeVisit is not null)
            throw new InvalidOperationException($"Visitor '{visitor.FullName}' is already checked in. Please check out first.");

        var visit = new Visit
        {
            VisitorId = dto.VisitorId,
            Purpose = dto.Purpose,
            HostName = dto.HostName,
            CheckInTime = DateTime.UtcNow,
            Status = VisitStatus.Active
        };

        var created = await _visitRepository.CreateAsync(visit);

        // Re-fetch to include Visitor navigation property
        var result = await _visitRepository.GetByIdAsync(created.Id);
        return MapToDto(result!);
    }

    public async Task<VisitResponseDto?> CheckOutAsync(int visitId)
    {
        var visit = await _visitRepository.GetByIdAsync(visitId);
        if (visit is null) return null;

        if (visit.Status == VisitStatus.Completed)
            throw new InvalidOperationException("This visit is already completed.");

        visit.CheckOutTime = DateTime.UtcNow;
        visit.Status = VisitStatus.Completed;

        var updated = await _visitRepository.UpdateAsync(visit);
        return MapToDto(updated);
    }

    public async Task<PagedResult<VisitResponseDto>> GetHistoryAsync(VisitSearchFilter filter)
    {
        var result = await _visitRepository.GetHistoryAsync(filter);

        return new PagedResult<VisitResponseDto>
        {
            Items = result.Items.Select(MapToDto).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<List<VisitResponseDto>> GetActiveVisitsAsync()
    {
        var visits = await _visitRepository.GetActiveVisitsAsync();
        return visits.Select(MapToDto).ToList();
    }

    public async Task<DailyReportDto> GetDailyReportAsync(DateTime date)
    {
        var visits = await _visitRepository.GetVisitsByDateAsync(date);

        return new DailyReportDto
        {
            ReportDate = date.Date,
            TotalVisits = visits.Count,
            ActiveVisits = visits.Count(v => v.Status == VisitStatus.Active),
            CompletedVisits = visits.Count(v => v.Status == VisitStatus.Completed),
            Visits = visits.Select(MapToDto).ToList()
        };
    }

    // ── Private mapper ──────────────────────────────────────────────────────
    private static VisitResponseDto MapToDto(Visit v) => new()
    {
        Id = v.Id,
        VisitorId = v.VisitorId,
        VisitorName = v.Visitor?.FullName ?? string.Empty,
        VisitorPhone = v.Visitor?.PhoneNumber ?? string.Empty,
        CheckInTime = v.CheckInTime,
        CheckOutTime = v.CheckOutTime,
        Purpose = v.Purpose,
        HostName = v.HostName,
        Status = v.Status.ToString(),
        DurationInMinutes = v.CheckOutTime.HasValue
            ? (v.CheckOutTime.Value - v.CheckInTime).TotalMinutes
            : null
    };
}