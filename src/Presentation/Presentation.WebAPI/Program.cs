using Application.BackgroundWorkers.Extensions;
using Application.BackgroundWorkers.Queue;
using Application.Handlers;
using Application.Handlers.Extensions;
using Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Configuration;
using Presentation.WebAPI.Exceptions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

PostgresConfiguration? postgresConfiguration = builder.Configuration
    .GetSection(nameof(PostgresConfiguration))
    .Get<PostgresConfiguration>() ?? throw new StartupException(nameof(PostgresConfiguration));

builder.Services.AddWorkersConfiguration(builder.Configuration);
builder.Services
    .AddHostedService<QueueActivityBackgroundWorker>()
    .AddHostedService<QueueAvailablePositionBackgroundWorker>();
builder.Services.AddSingleton(postgresConfiguration);
builder.Services.AddDatabase(o =>
{
    o.UseNpgsql(postgresConfiguration.ToConnectionString("default"));
});

builder.Services.AddApplication();
builder.Services.AddEndpoints();

WebApplication app = builder.Build();

using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDatabase();
}

app.UseEndpoints();
app.Run();