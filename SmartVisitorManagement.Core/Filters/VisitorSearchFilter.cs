namespace SmartVisitorManagement.Core.Filters;

public class VisitorSearchFilter
{
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}