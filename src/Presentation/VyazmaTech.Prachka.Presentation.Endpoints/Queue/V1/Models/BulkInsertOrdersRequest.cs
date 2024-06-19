namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

internal sealed record BulkInsertOrdersRequest(Guid QueueId, int Quantity);