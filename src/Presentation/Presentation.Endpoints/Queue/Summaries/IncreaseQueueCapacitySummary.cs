using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.Queue.IncreaseQueueCapacity;
using static Application.Handlers.Queue.Commands.IncreaseQueueCapacity.IncreaseQueueCapacity;

namespace Presentation.Endpoints.Queue.Summaries;

internal sealed class IncreaseQueueCapacitySummary : Summary<IncreaseQueueCapacityEndpoint>
{
    public IncreaseQueueCapacitySummary()
    {
        Summary = "Increase queue capacity.";
        Description = "This endpoint increases queue capacity. Queue capacity should not be less than current.";
        Response<Response>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}