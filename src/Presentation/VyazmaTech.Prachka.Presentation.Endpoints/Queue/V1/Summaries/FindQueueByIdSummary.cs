using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.Queue;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Summaries;

internal sealed class FindQueueByIdSummary : Summary<FindQueueByIdEndpoint>
{
    public FindQueueByIdSummary()
    {
        Summary = "Find queue by id";
        Response<QueueWithOrdersDto>();
        Response<ProblemDetails>(StatusCodes.Status404NotFound, "Queue with specified id was not found");
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Id was not in correct format");
    }
}