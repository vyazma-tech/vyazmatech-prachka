using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.Order.FindOrders;
using static Application.Handlers.Order.Queries.OrderById.OrderByIdQuery;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.Endpoints.Order.Summaries;

internal sealed class FindOrderByIdSummary : Summary<FindOrderByIdEndpoint>
{
    public FindOrderByIdSummary()
    {
        Summary = "Find order by id";
        Response<Response>();
        Response<ProblemDetails>(StatusCodes.Status404NotFound, "Order with specified id was not found");
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Id was not in correct format");
    }
}