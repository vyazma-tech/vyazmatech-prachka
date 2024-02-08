using FastEndpoints.Swagger;
using Infrastructure.Authentication.Extensions;
using Infrastructure.DataAccess.Extensions;
using Presentation.Authentication.Extensions;
using Presentation.Authorization;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();
builder.Configuration.AddJsonFile("features.json");

builder.Services
    .AddWorkers(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddCachePolicy(builder.Configuration)
    .AddIdentityConfiguration(builder.Configuration)
    .AddTelegramAuthentication()
    .AddVyazmaTechAuthorization();

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

app.UseOutputCache();
await app.RunAsync();