using VyazmaTech.Prachka.Presentation.Hubs.Queue.Implementations;

namespace VyazmaTech.Prachka.Presentation.Hubs.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder builder)
    {
        builder.MapHub<QueueHub>("/hub");
        return builder;
    }
}