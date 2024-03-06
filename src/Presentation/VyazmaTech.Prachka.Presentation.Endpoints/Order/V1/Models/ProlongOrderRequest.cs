namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Models;

internal sealed record ProlongOrderRequest(Guid OrderId, Guid QueueId);