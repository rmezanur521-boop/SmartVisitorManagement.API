using System.ComponentModel.DataAnnotations;

namespace SmartVisitorManagement.Core.DTOs.Visit;

public class CheckInDto
{
    [Required(ErrorMessage = "Visitor ID is required.")]
    public int VisitorId { get; set; }

    [Required(ErrorMessage = "Purpose is required.")]
    [MaxLength(250)]
    public string Purpose { get; set; } = string.Empty;

    [Required(ErrorMessage = "Host name is required.")]
    [MaxLength(100)]
    public string HostName { get; set; } = string.Empty;
}