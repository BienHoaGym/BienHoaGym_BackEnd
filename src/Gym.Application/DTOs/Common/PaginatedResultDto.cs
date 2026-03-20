namespace Gym.Application.DTOs.Common;

/// <summary>
/// Generic paginated result wrapper
/// </summary>
public class PaginatedResultDto<T>
{
    public List<T> Items { get; set; } = new();

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}