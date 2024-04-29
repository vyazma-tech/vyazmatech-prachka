namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

internal sealed record BulkRemoveOrdersRequest(Guid QueueId, int Quantity);