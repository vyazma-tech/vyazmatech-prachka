using Microsoft.AspNetCore.Mvc.Testing;
using TrusovNET.Playground;
using Xunit;

namespace Test.Common.Fixtures.WebFactory;

public class WebAppFactory : WebApplicationFactory<IPlaygroundMarker>, IAsyncLifetime
{
    protected HttpClient Client { get; private set; } = default!;

    public Task InitializeAsync()
    {
        Client = CreateClient();

        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        Client.Dispose();

        return Task.CompletedTask;
    }
}