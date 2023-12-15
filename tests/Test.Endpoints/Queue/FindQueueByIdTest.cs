using System.Collections.ObjectModel;
using System.Net;
using Application.Handlers.Queue.Queries;
using Domain.Kernel;
using FastEndpoints;
using FluentAssertions;
using Infrastructure.DataAccess.Quering.Abstractions;
using Infrastructure.Tools;
using Presentation.Endpoints.Queue.FindQueue;
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
        string queueId = Guid.NewGuid().ToString();
        IList<QueryParameter<QueueQueryParameter>> queryParameters = new[]
        {
            new QueryParameter<QueueQueryParameter>
            (
                QueueQueryParameter.QueueId,
                queueId
            )
        };
        var readOnlyCollection = new ReadOnlyCollection<QueryParameter<QueueQueryParameter>>(queryParameters);
        var conf = new QueryConfiguration<QueueQueryParameter>(readOnlyCollection);
        TestResult<QueueResponse> response = await _client
            .GETAsync<FindQueueEndpoint, QueryConfiguration<QueueQueryParameter>, QueueResponse>(conf);

        response.Result.Should().BeNull();
        response.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}