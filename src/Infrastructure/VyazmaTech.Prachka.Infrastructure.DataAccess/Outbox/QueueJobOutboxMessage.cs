namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Outbox;

public sealed record QueueJobOutboxMessage
{
    public Guid QueueId { get; init; }

    public string JobId { get; set; } = string.Empty;

    public DateTime OccuredOnUtc { get; init; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }
}