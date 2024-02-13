namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.Models;

internal sealed record IncreaseCapacityRequest(Guid QueueId, int Capacity);
