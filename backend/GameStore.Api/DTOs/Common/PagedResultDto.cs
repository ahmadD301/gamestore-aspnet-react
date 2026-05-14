namespace GameStore.Api.DTOs.Common;

public sealed class PagedResultDto<T>
{
    public IReadOnlyCollection<T> Items { get; set; }
        = [];

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}