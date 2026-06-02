using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.DTOs.Visit;
using SmartVisitorManagement.Core.Filters;

namespace SmartVisitorManagement.Core.Interfaces.Services;

public interface IVisitService
{
    Task<VisitResponseDto> CheckInAsync(CheckInDto dto);
    Task<VisitResponseDto?> CheckOutAsync(int visitId);
    Task<PagedResult<VisitResponseDto>> GetHistoryAsync(VisitSearchFilter filter);
    Task<List<VisitResponseDto>> GetActiveVisitsAsync();
    Task<DailyReportDto> GetDailyReportAsync(DateTime date);
}