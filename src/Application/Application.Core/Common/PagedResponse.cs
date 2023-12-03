namespace Application.Core.Common;

public sealed class PagedResponse<T>
{
    public int RecordPerPage { get; set; }
    public int CurrentPage { get; set; }
    public long TotalPages { get; set; }
    public IReadOnlyCollection<T> Bunch { get; set; } = Array.Empty<T>();
}