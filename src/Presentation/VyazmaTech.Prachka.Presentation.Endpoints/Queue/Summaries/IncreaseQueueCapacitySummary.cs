using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.Queue;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.Summaries;

internal sealed class IncreaseQueueCapacitySummary : Summary<IncreaseQueueCapacityEndpoint>
{
    public IncreaseQueueCapacitySummary()
    {
        Summary = "Increase queue capacity.";
        Description = "This endpoint increases queue capacity. Queue capacity should not be less than current.";
        Response<QueueDto>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}