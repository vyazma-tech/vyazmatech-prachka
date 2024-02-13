namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.Models;

internal sealed record FindOrdersRequest(
    Guid? UserId,
    Guid? QueueId,
    DateTime? CreationDate,
    string? Status,
    int? Page);