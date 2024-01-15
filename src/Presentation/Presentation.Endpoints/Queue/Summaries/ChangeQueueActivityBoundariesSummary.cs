using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.Queue.ChangeQueueActivityBoundaries;
using static Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries.ChangeQueueActivityBoundaries;

namespace Presentation.Endpoints.Queue.Summaries;

internal sealed class ChangeQueueActivityBoundariesSummary : Summary<ChangeQueueActivityBoundariesEndpoint>
{
    public ChangeQueueActivityBoundariesSummary()
    {
        Summary = "Changes queue activity boundaries.";
        Description = "This endpoint changes queue activity boundaries.";
        Response<Response>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}