namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Models;

internal sealed record MarkAsPaidRequest(Guid OrderId, double Price);