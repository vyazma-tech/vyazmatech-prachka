using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Reports;
using VyazmaTech.Prachka.Application.Dto.Reports;
using VyazmaTech.Prachka.Presentation.Authorization;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Reports.V1;

internal sealed record GetReportsRequest(DateOnly From, DateOnly To);

internal sealed class GetReportsEndpoint : Endpoint<GetReportsRequest, ReportDto>
{
    private const string FeatureName = "GetReports";
    private readonly ISender _sender;

    public GetReportsEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<ReportsEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(GetReportsRequest req, CancellationToken ct)
    {
        var query = new GetReport.Query(req.From, req.To);
        var response = await _sender.Send(query, ct);

        await SendBytesAsync(
            response.binaryReport,
            $"report-{req.From.ToString("Y")}-{req.To.ToString("Y")}",
            "application/pdf",
            cancellation: ct);
    }
}