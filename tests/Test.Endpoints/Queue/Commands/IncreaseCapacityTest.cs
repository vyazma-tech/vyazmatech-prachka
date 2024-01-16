using System.Net;
using Application.Core.Common;
using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Application.Handlers.Queue.Queries;
using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.Queue.IncreaseQueueCapacity;
using Test.Endpoints.Fixtures.WebFactory;
using Xunit;

namespace Test.Endpoints.Queue.Commands;

[Collection(nameof(WebAppFactoryCollectionFixture))]
public class IncreaseCapacityTest
{
    private readonly HttpClient _client;
    
    public IncreaseCapacityTest(WebAppFactory factory)
    {
        _client = factory.Client;
    }

    [Fact]
    public async Task IncreaseCapacity_ShouldReturnFailure_WhenNewCapacityLessThanPrevious()
    {
        var cmd = new IncreaseQueueCapacityCommand(Guid.NewGuid(), 15);
        TestResult<ResultResponse<QueueResponse>> response = await _client
            .PATCHAsync<IncreaseQueueCapacityEndpoint, IncreaseQueueCapacityCommand, ResultResponse<QueueResponse>>(cmd);

        response.Should().NotBeNull();
        response.Response.Should().NotBeNull();
        response.Response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status500InternalServerError);
    }
}