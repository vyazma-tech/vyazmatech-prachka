namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

internal sealed record IncreaseCapacityRequest(Guid QueueId, int Capacity);
