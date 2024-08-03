using VyazmaTech.Platform.Rtc.Extensions;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;
using VyazmaTech.Prachka.Application.Handlers.Extensions;
using VyazmaTech.Prachka.Infrastructure.Authentication.Extensions;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;
using VyazmaTech.Prachka.Infrastructure.Jobs.Extensions;
using VyazmaTech.Prachka.Presentation.Authentication.Extensions;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;
using VyazmaTech.Prachka.Presentation.WebAPI.Extensions;
using VyazmaTech.Prachka.Presentation.WebAPI.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();
builder.Configuration.AddJsonFile("features.json");
builder.AddRtc();
await SecretConfigurationBuilder.AddConfiguration(builder.Environment, builder.Configuration);

builder.Services
    .AddInfrastructure()
    .AddPostgresConfiguration(builder.Configuration)
    .AddDatabase()
    .AddJobs(builder.Configuration)
    .AddOutbox(builder.Configuration);

builder.Services
    .AddIdentityConfiguration(builder.Configuration);

builder.Services
    .AddTelegramAuthentication()
    .AddVyazmaTechAuthorization();

builder.Services
    .AddApplication(builder.Configuration)
    .AddHandlers()
    .AddMiddlewares()
    .AddEndpoints();

builder.Services.AddCors(o => o
    .AddDefaultPolicy(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));

WebApplication app = builder.Build().ConfigureApp();
await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDatabase();
    await scope.SeedRoles();
    await scope.SeedDefaultAdmins(builder.Configuration);
}

await app.RunAsync();