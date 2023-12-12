using Application.Core.Configuration;
using Application.Handlers.Extensions;
using FastEndpoints.Swagger;
using Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Configuration;
using Presentation.WebAPI.Exceptions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

PostgresConfiguration? postgresConfiguration = builder.Configuration
    .GetSection(nameof(PostgresConfiguration))
    .Get<PostgresConfiguration>() ?? throw new StartupException(nameof(PostgresConfiguration));

builder.Services.AddSingleton(postgresConfiguration);
builder.Services.AddDatabase(o =>
{
    o.UseNpgsql(postgresConfiguration.ToConnectionString("trusov_net"))
        .UseLazyLoadingProxies();
});

builder.Services.Configure<PaginationConfiguration>(
    builder.Configuration.GetSection(PaginationConfiguration.SectionKey));

builder.Services
    .AddFilterChains()
    .AddQueryChains()
    .AddApplication()
    .SwaggerDocument();

builder.Services.AddEndpoints();

WebApplication app = builder.Build();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDatabase();
}

app
    .UseEndpoints()
    .UseSwaggerGen();

app.Run();