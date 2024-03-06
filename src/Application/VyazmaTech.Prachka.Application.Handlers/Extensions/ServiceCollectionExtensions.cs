using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.Core.PreProcessors;

namespace VyazmaTech.Prachka.Application.Handlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediator(s =>
        {
            s.Namespace = "VyazmaTech.Prachka.Application.Handlers";
            s.ServiceLifetime = ServiceLifetime.Transient;
        });

        services.AddValidatorsFromAssembly(IApplicationHandlersMarker.Assembly);

        return services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPreProcessor<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPreprocessor<,>));
    }
}