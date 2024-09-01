namespace Aram.BFF.Contracts.Common;

public class PaginatedResponse<T>(IEnumerable<T> items, int totalRecords, int totalPages)
{
    public IEnumerable<T> Items { get; } = items;
    public int TotalRecords { get; } = totalRecords;
    public int TotalPages { get; } = totalPages;
}