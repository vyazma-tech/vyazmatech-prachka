using Application.Core.Behaviours;
using Application.Handlers.Order.Queries;
using Application.Handlers.Queue.Queries;
using Application.Handlers.User.Queries;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using FluentChaining;
using FluentChaining.Configurators;
using FluentValidation;
using Infrastructure.DataAccess.Quering.Abstractions;
using Infrastructure.DataAccess.Quering.Adapters;
using Infrastructure.DataAccess.Quering.Requests;
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
        services.AddModelQuery<UserQuery.QueryBuilder, UserQueryParameter>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<UserQuery.QueryBuilder, UserQueryParameter>();
        
        services.AddModelQuery<OrderQuery.QueryBuilder, OrderQueryParameter>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<OrderQuery.QueryBuilder, OrderQueryParameter>();
        
        services.AddModelQuery<QueueQuery.QueryBuilder, QueueQueryParameter>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<QueueQuery.QueryBuilder, QueueQueryParameter>();

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

        services.AddModelFilter<QueueEntity, QueueQueryParameter>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddFilterChain<QueueEntity, QueueQueryParameter>();
        
        return services;
    }
    
    private static void AddModelFilter<TValue, TParameter>(this IServiceCollection services)
    {
        services.AddSingleton<IModelFilter<TValue, TParameter>, ModelFilterAdapter<TValue, TParameter>>();
    }

    private static void AddModelQuery<TValue, TParameter>(this IServiceCollection services)
    {
        services.AddSingleton<IModelQuery<TValue, TParameter>, ModelQueryAdapter<TValue, TParameter>>();
    }

    private static IChainConfigurator AddFilterChain<TValue, TParameter>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<ModelFilterRequest<TValue, TParameter>, IEnumerable<TValue>>(x => x
            .ThenFromAssemblies(typeof(ServiceCollectionExtensions).Assembly)
            .FinishWith((r, _) => r.data));
    }

    private static IChainConfigurator AddQueryChain<TValue, TParameter>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<ModelQueryRequest<TValue, TParameter>, TValue>(x => x
            .ThenFromAssemblies(typeof(ServiceCollectionExtensions).Assembly)
            .FinishWith((r, _) => r.queryBuilder));
    }
}