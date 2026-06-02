namespace SmartVisitorManagement.Core.DTOs.Visit;

public class DailyReportDto
{
    public DateTime ReportDate { get; set; }
    public int TotalVisits { get; set; }
    public int ActiveVisits { get; set; }
    public int CompletedVisits { get; set; }
    public List<VisitResponseDto> Visits { get; set; } = new();
}