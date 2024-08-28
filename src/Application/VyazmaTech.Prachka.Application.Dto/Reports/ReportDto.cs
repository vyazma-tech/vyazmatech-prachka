namespace VyazmaTech.Prachka.Application.Dto.Reports;

public sealed record ReportDto(DateOnly ReportFrom, DateOnly ReportDue, IReadOnlyList<ReportModel> Reports);

public sealed record ReportModel(IReadOnlyCollection<ReportLineItem> LineItems);

public sealed record ReportLineItem(Guid UserId, string Fullname, double OrderPrice, DateTime CreatedAt);