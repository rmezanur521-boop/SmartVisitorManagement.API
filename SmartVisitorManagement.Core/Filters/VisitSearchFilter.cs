namespace SmartVisitorManagement.Core.Filters;

public class VisitSearchFilter
{
    public DateTime? Date { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}