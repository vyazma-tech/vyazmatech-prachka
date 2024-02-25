namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Models;

internal sealed record FindOrdersRequest(
    Guid? UserId,
    Guid? QueueId,
    DateTime? CreationDate,
    string? Status,
    int? Page);