namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

internal sealed record ChangeActivityBoundariesRequest(Guid QueueId, TimeOnly ActiveFrom, TimeOnly ActiveUntil);