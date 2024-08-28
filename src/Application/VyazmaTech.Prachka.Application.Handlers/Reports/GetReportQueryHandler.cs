using HTMLQuestPDF.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.Reports;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Reports.GetReport;

namespace VyazmaTech.Prachka.Application.Handlers.Reports;

internal sealed class GetReportQueryHandler : IQueryHandler<Query, Response>
{
    private readonly ReportFactory _reportFactory;
    private readonly IPersistenceContext _context;

    public GetReportQueryHandler(IPersistenceContext context)
    {
        _context = context;
        _reportFactory = new ReportFactory();
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var reports = await _context.Reports.GetReports(request.From, request.To);

        var from = reports.FirstOrDefault()?.LineItems.FirstOrDefault()?.CreatedAt.AsDateOnly()
                   ?? request.From;
        var to = reports.LastOrDefault()?.LineItems.FirstOrDefault()?.CreatedAt.AsDateOnly()
                 ?? request.To;

        var dto = new ReportDto(from, to, reports);
        var report = await _reportFactory.CreateAsync(dto);

        var binaryReport = GenerateReport(report);

        return new Response(binaryReport);
    }

    private byte[] GenerateReport(string htmlReport)
    {
        return Document.Create(container =>
                container.Page(page =>
                    page.Content()
                        .Column(col =>
                            col.Item()
                                .HTML(handler =>
                                {
                                    handler.SetHtml(htmlReport);

                                    handler.SetContainerStyleForHtmlElement(
                                        "html",
                                        style => style
                                            .Background("#F5F7FA"));

                                    handler.SetContainerStyleForHtmlElement(
                                        "div",
                                        style => style.Padding(25));

                                    handler.SetContainerStyleForHtmlElement(
                                        "div.table-container",
                                        style => style
                                            .Background("#ffffff")
                                            .BorderColor("#D1D5DB"));

                                    handler.SetTextStyleForHtmlElement(
                                        "h1",
                                        TextStyle.Default
                                            .FontColor(Colors.BlueGrey.Darken3)
                                            .FontFamily("Helvetica")
                                            .FontSize(20)
                                            .ExtraBold());

                                    handler.SetTextStyleForHtmlElement(
                                        "h3",
                                        TextStyle.Default
                                            .FontColor(Colors.BlueGrey.Darken1)
                                            .FontFamily("Helvetica")
                                            .FontSize(16)
                                            .Bold());

                                    handler.SetTextStyleForHtmlElement(
                                        "th",
                                        TextStyle.Default
                                            .FontColor(Color.FromHex("#001122a7"))
                                            .FontFamily("Helvetica")
                                            .FontSize(12)
                                            .Bold());

                                    handler.SetTextStyleForHtmlElement(
                                        "td",
                                        TextStyle.Default
                                            .FontColor(Colors.BlueGrey.Darken2)
                                            .FontFamily("Helvetica")
                                            .FontSize(12));

                                    handler.SetContainerStyleForHtmlElement(
                                        "table",
                                        style => style
                                            .Background("#ffffff"));
                                }))))
            .GeneratePdf();
    }
}