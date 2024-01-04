using Application.Core.Behaviours;
using Application.Core.Querying.Abstractions;
using Application.Core.Querying.Adapters;
using Application.Core.Querying.Requests;
using Application.Handlers.Order.Queries;
using Application.Handlers.Queue.Queries;
using Application.Handlers.User.Queries;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using FluentChaining;
using FluentChaining.Configurators;
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

    public static IServiceCollection AddQueryChains(this IServiceCollection services)
    {
        services.AddModelQuery<IQueryable<UserEntity>, UserQuery>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<IQueryable<UserEntity>, UserQuery>();

        services.AddModelQuery<IQueryable<OrderEntity>, OrderQuery>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<IQueryable<OrderEntity>, OrderQuery>();

        services.AddModelQuery<IQueryable<QueueEntity>, QueueQuery>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<IQueryable<QueueEntity>, QueueQuery>();

        return services;
    }

    public static IServiceCollection AddFilterChains(this IServiceCollection services)
    {
        services.AddModelFilter<UserEntity, UserQueryParameter>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddFilterChain<UserEntity, UserQueryParameter>();

        services.AddModelFilter<OrderEntity, OrderQueryParameter>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddFilterChain<OrderEntity, OrderQueryParameter>();

        // TODO: filtering
        // services.AddModelFilter<QueueEntity, QueueQueryParameter>();
        // services
        //     .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
        //     .AddFilterChain<QueueEntity, QueueQueryParameter>();
        return services;
    }

    private static void AddModelFilter<TValue, TParameter>(this IServiceCollection services)
    {
        services.AddSingleton<IEntityFilter<TValue, TParameter>, EntityFilterAdapter<TValue, TParameter>>();
    }

    private static void AddModelQuery<TValue, TParameter>(this IServiceCollection services)
    {
        services.AddSingleton<IEntityQuery<TValue, TParameter>, EntityQueryAdapter<TValue, TParameter>>();
    }

    private static IChainConfigurator AddFilterChain<TValue, TParameter>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<EntityFilterRequest<TValue, TParameter>, IEnumerable<TValue>>(x => x
            .ThenFromAssemblies(typeof(ServiceCollectionExtensions).Assembly)
            .FinishWith((r, _) => r.Data));
    }

    private static IChainConfigurator AddQueryChain<TValue, TParameter>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<EntityQueryRequest<TValue, TParameter>, TValue>(x => x
            .ThenFromAssemblies(typeof(ServiceCollectionExtensions).Assembly)
            .FinishWith((r, _) => r.QueryBuilder));
    }
}