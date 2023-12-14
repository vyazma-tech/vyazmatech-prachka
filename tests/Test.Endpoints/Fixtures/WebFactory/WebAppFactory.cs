using Infrastructure.DataAccess.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebAPI;
using Xunit;

namespace Test.Endpoints.Fixtures.WebFactory;

public class WebAppFactory : WebApplicationFactory<IWebAPIMarker>, IAsyncLifetime
{
    public HttpClient Client { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(s =>
        {
            ServiceDescriptor? descriptor = s.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

            if (descriptor is not null)
                s.Remove(descriptor);

            s.AddDbContext<DatabaseContext>(x =>
                x.UseSqlite("Data Source=staging.db"));
        });

        builder.UseEnvironment("Staging");
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