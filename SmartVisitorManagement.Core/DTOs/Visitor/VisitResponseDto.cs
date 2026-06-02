using SmartVisitorManagement.Core.Common;

namespace SmartVisitorManagement.Core.DTOs.Visit;

public class VisitResponseDto
{
    public int Id { get; set; }
    public int VisitorId { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string VisitorPhone { get; set; } = string.Empty;
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double? DurationInMinutes { get; set; }
}