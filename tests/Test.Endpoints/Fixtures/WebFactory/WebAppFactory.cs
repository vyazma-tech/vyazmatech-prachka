using FastEndpoints;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TrusovNET.Playground;
using Xunit;

namespace Test.Endpoints.Fixtures.WebFactory;

public class WebAppFactory : WebApplicationFactory<IPlaygroundMarker>, IAsyncLifetime
{
    protected HttpClient Client { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(s =>
        {
            s.AddFastEndpoints();
        });
    }

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