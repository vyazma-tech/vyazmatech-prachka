using Razor.Templating.Core;
using VyazmaTech.Prachka.Application.Dto.Reports;

namespace VyazmaTech.Prachka.Application.Handlers.Reports;

internal sealed class ReportFactory
{
    public Task<string> CreateAsync(ReportDto reportDto)
    {
        return RazorTemplateEngine.RenderAsync("Reports/Report.cshtml", reportDto);
    }
}