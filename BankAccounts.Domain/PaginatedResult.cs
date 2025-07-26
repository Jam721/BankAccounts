using System.Diagnostics.CodeAnalysis;

namespace BankAccounts.Domain;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class PaginatedResult<T>(List<T> items, int totalCount, int pageNumber, int pageSize)
{
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public List<T> Items { get; set; } = items;
}