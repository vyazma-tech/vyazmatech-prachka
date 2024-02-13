using FastEndpoints.Swagger;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;
using VyazmaTech.Prachka.Application.Handlers.Extensions;
using VyazmaTech.Prachka.Infrastructure.Authentication.Extensions;
using VyazmaTech.Prachka.Infrastructure.Caching;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;
using VyazmaTech.Prachka.Presentation.Authentication.Extensions;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.WebAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();
builder.Configuration.AddJsonFile("features.json");

builder.Services
    .AddWorkersConfiguration(builder.Configuration)
    .AddWorkers();

builder.Services
    .AddInfrastructure()
    .AddPostgresConfiguration(builder.Configuration)
    .AddDatabase();

builder.Services
    .AddIdentityConfiguration(builder.Configuration);

builder.Services
    .AddCaching(builder.Configuration)
    .AddTelegramAuthentication()
    .AddVyazmaTechAuthorization();

builder.Services
    .AddApplication(builder.Configuration)
    .AddHandlers()
    .AddMiddlewares()
    .AddEndpoints()
    .SwaggerDocument();

WebApplication app = builder.Build().ConfigureApp();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDatabase();
}

await app.RunAsync();