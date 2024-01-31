using FastEndpoints.Swagger;
using Infrastructure.Authentication.Extensions;
using Infrastructure.DataAccess.Extensions;
using Presentation.Authentication.Extensions;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();

builder.Services
    .AddWorkers(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddIdentityConfiguration(builder.Configuration)
    .AddTelegramAuthentication();

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