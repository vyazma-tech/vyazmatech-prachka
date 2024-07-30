namespace VyazmaTech.Prachka.Application.Dto.Core.Order;

public sealed record MyOrdersDto(
    DateOnly Date,
    IReadOnlyCollection<MyOrdersOrderModel> Orders);

public sealed record MyOrdersOrderModel(
    string Status,
    string? Comment,
    DateOnly CreationDate,
    DateTime? ModificationDate,
    bool isNotifyAvailable);