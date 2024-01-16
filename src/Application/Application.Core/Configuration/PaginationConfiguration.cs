namespace Application.Core.Configuration;

public class PaginationConfiguration
{
    public const string SectionKey = nameof(PaginationConfiguration);
    public int RecordsPerPage { get; set; }
}