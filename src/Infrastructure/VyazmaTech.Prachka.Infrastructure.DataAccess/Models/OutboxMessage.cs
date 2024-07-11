namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

public sealed class OutboxMessage
{
    public long Id { get; init; }

    public string Type { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public DateTime OccuredOnUtc { get; init; }

    public DateTime? ProcessedOnUtc { get; set; } = null;

    public string? Error { get; set; } = null;
}