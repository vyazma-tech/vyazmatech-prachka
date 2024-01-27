using FastEndpoints.Swagger;
using Infrastructure.DataAccess.Extensions;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();

builder.Services
    .AddWorkers(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddRedisCache(builder.Configuration);

builder.Services
    .AddApplication(builder.Configuration)
    .AddGlobalExceptionHandler()
    .AddEndpoints()
    .SwaggerDocument();

WebApplication app = builder.Build().ConfigureApp();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDatabase();
}

await app.RunAsync();