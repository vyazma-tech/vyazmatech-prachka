using Application.Core.Common;
using Application.Handlers.Queue.Queries;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Queue.IncreaseQueueCapacity;

// TODO: FIX IT
// public class IncreaseQueueCapacityEndpoint : Endpoint<IncreaseQueueCapacityCommand, ResultResponse<QueueResponse>>
// {
//     private readonly IMediator _mediator;
//
//     public IncreaseQueueCapacityEndpoint(IMediator mediator)
//     {
//         _mediator = mediator;
//     }
//
//     public override void Configure()
//     {
//         Verbs(Http.PATCH);
//         Routes("api/queue/increase-capacity");
//         AllowAnonymous();
//     }
//
//     public override async Task HandleAsync(IncreaseQueueCapacityCommand req, CancellationToken ct)
//     {
//         ResultResponse<QueueResponse> response = await _mediator.Send(req, ct);
//
//         if (response.IsSuccess)
//         {
//             await SendOkAsync(response, ct);
//         }
//         else
//         {
//             await SendAsync(response, StatusCodes.Status400BadRequest, ct);
//         }
//     }
// }