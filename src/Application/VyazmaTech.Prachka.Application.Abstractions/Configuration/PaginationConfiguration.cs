namespace VyazmaTech.Prachka.Application.Abstractions.Configuration;

public class PaginationConfiguration
{
    public const string SectionKey = "Application:PaginationConfiguration";
    public int RecordsPerPage { get; set; }
}