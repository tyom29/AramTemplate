namespace Aram.BFF.Application.Common.Models;

public class PaginatedResult<T>(IEnumerable<T> items, int totalRecords, int pageSize)
{
    public IEnumerable<T> Items { get; set; } = items;
    public int TotalRecords { get; set; } = totalRecords;
    public int TotalPages { get; set; } = (int)Math.Ceiling(totalRecords / (double)pageSize);
}