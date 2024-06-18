namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

internal sealed record FindQueuesRequest(DateOnly? SearchFrom, int Page, int? Limit);