using Application.Handlers;
using Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpoints();
builder.Services.AddApplication();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseEndpoints();
app.Run();