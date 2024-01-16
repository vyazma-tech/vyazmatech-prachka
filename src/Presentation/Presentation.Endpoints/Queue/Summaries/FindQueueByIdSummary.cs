using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.Queue.FindQueue;
using static Application.Handlers.Queue.Queries.QueueById.QueueByIdQuery;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.Endpoints.Queue.Summaries;

internal sealed class FindQueueByIdSummary : Summary<FindQueueByIdEndpoint>
{
    public FindQueueByIdSummary()
    {
        Summary = "Find queue by id";
        Response<Response>();
        Response<ProblemDetails>(StatusCodes.Status404NotFound, "Queue with specified id was not found");
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Id was not in correct format");
    }
}