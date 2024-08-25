using FastEndpoints;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Reports;

internal sealed class ReportsEndpointGroup : Group
{
    public ReportsEndpointGroup()
    {
        Configure("reports", _ => { });
    }
}