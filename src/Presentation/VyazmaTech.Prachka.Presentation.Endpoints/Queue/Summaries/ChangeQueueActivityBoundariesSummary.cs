using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.Summaries;

internal sealed class ChangeQueueActivityBoundariesSummary : Summary<ChangeQueueActivityBoundariesEndpoint>
{
    public ChangeQueueActivityBoundariesSummary()
    {
        Summary = "Changes queue activity boundaries.";
        Description = "This endpoint changes queue activity boundaries.";
        Response<ChangeActivityBoundariesRequest>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}