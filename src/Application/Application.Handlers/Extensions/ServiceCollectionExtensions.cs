using Application.Core.Behaviours;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Handlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(s =>
        {
            s.Namespace = "Application.Handlers";
            s.ServiceLifetime = ServiceLifetime.Transient;
        });

        services.AddValidatorsFromAssembly(IApplicationHandlersMarker.Assembly);

        return services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}