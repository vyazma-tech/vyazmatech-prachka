using FastEndpoints;
using Mediator;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Contracts.Core.Queues.Queries;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Core.Queue;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal class FindQueuesEndpoint : Endpoint<FindQueuesRequest, PagedResponse<QueueDto>>
{
    private readonly ISender _sender;
    private readonly IDateTimeProvider _timeProvider;
    private readonly int _recordPerPage;

    public FindQueuesEndpoint(
        ISender sender,
        IOptions<PaginationConfiguration> paginationConfiguration,
        IDateTimeProvider timeProvider)
    {
        _sender = sender;
        _timeProvider = timeProvider;
        _recordPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
        Group<QueueEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(FindQueuesRequest req, CancellationToken ct)
    {
        var query = new QueueByQuery.Query(
            SearchFrom: req.SearchFrom ?? _timeProvider.DateNow,
            Page: req.Page ?? 0,
            Limit: req.Limit ?? _recordPerPage);

        QueueByQuery.Response response = await _sender.Send(query, ct);

        if (response.Queues.Bunch.Any())
        {
            await this.SendPartialContentAsync(response, ct);
        }
        else
        {
            await SendNoContentAsync(ct);
        }
    }
}