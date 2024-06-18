using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.Core.Queue;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Summaries;

internal sealed class ChangeQueueActivityBoundariesSummary : Summary<ChangeQueueActivityBoundariesEndpoint>
{
    public ChangeQueueActivityBoundariesSummary()
    {
        Summary = "Changes queue activity boundaries.";
        Description = "This endpoint changes queue activity boundaries.";
        Response<QueueDto>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}