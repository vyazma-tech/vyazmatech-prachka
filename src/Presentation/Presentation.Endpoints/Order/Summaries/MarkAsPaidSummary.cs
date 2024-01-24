using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.Order.MarkAsPaid;
using static Application.Core.Contracts.Orders.Commands.MarkOrderAsPaid;
namespace Presentation.Endpoints.Order.Summaries;

internal sealed class MarkAsPaidSummary : Summary<MarkAsPaidEndpoint>
{
    public MarkAsPaidSummary()
    {
        Summary = "Mark order as paid.";
        Description = "This endpoint marks order with specified id as paid.";
        Response<Response>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}