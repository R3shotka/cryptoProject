namespace api.Helpers;

public class QueryObject
{
    public string? Symbol { get; set; } = null;
    public string? Name { get; set; } = null;
    
    public string? SortBy { get; set; } = null;
    public bool IsDescending { get; set; } = false;
    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 1;
}