using SmartVisitorManagement.Core.Common;

namespace SmartVisitorManagement.Core.Entities;

public class Visit
{
    public int Id { get; set; }
    public int VisitorId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public VisitStatus Status { get; set; } = VisitStatus.Active;

    // Navigation property
    public Visitor Visitor { get; set; } = null!;
}