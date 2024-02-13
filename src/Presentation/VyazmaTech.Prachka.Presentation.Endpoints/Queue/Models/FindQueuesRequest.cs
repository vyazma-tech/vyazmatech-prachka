namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.Models;

internal sealed record FindQueuesRequest(DateOnly? AssignmentDate, Guid? OrderId, int? Page);
