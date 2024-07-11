namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

public sealed record QueueJobMessage
{
    public long Id { get; init; }

    public Guid QueueId { get; init; }

    public string JobId { get; init; } = string.Empty;
}