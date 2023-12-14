using Application.Handlers.Queue.Queries;
using FastEndpoints;
using Infrastructure.DataAccess.Quering.Abstractions;
using Mediator;

namespace Presentation.Endpoints.Queue.FindQueue;

public class FindQueueEndpoint : Endpoint<QueryConfiguration<QueueQueryParameter>, QueueResponse>
{
    private readonly IMediator _mediator;
    private readonly IModelQuery<QueueQuery.QueryBuilder, QueueQueryParameter> _query;

    public FindQueueEndpoint(IMediator mediator, IModelQuery<QueueQuery.QueryBuilder, QueueQueryParameter> query)
    {
        _mediator = mediator;
        _query = query;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/queue/query");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QueryConfiguration<QueueQueryParameter> req, CancellationToken ct)
    {
        QueueQuery.QueryBuilder queryBuilder = QueueQuery.Builder;
        queryBuilder = _query.Apply(queryBuilder, req);

        QueueQuery queueQuery = queryBuilder.Build();

        QueueResponse response = await _mediator.Send(queueQuery, ct);
        await SendOkAsync(response, ct);
    }
}