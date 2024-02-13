namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.Models;

internal sealed record ChangeActivityBoundariesRequest(Guid QueueId, TimeOnly ActiveFrom, TimeOnly ActiveUntil);