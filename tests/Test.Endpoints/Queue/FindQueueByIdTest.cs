using System.Net;
using Application.Handlers.Queue.Commands.CreateQueue;
using Application.Handlers.Queue.Queries;
using Application.Handlers.Queue.Queries.FindByIdQueue;
using Domain.Kernel;
using FastEndpoints;
using FluentAssertions;
using Infrastructure.Tools;
using Presentation.Endpoints.Queue.CreateQueues;
using Presentation.Endpoints.Queue.FindQueues;
using Test.Endpoints.Fixtures.WebFactory;
using Xunit;

namespace Test.Endpoints.Queue;

[Collection(nameof(WebAppFactoryCollectionFixture))]
public class FindQueueByIdTest
{
    private readonly HttpClient _client;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public FindQueueByIdTest(WebAppFactory factory)
    {
        _client = factory.Client;
        _dateTimeProvider = new DateTimeProvider();
    }

    [Fact]
    public async Task TryFindQueueByIdRequest_ShouldReturnNotFound_WhenNothingAddedToDB()
    {
        FindQueueByIdQuery query = new FindQueueByIdQuery(Guid.NewGuid());
        TestResult<QueueResponse> response = await _client
            .GETAsync<FindQueueByIdEndpoint, FindQueueByIdQuery, QueueResponse>(query);

        response.Result.Should().BeNull();
        response.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}