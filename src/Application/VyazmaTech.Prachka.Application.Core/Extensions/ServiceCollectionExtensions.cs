using FluentChaining;
using FluentChaining.Configurators;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.Abstractions.Querying.OrderSubscription;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.Abstractions.Querying.QueueSubscription;
using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.Contracts.Orders.Queries;
using VyazmaTech.Prachka.Application.Contracts.Queues.Queries;
using VyazmaTech.Prachka.Application.Contracts.Users.Queries;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using VyazmaTech.Prachka.Application.Core.Users;
using IOrderQueryBuilder = VyazmaTech.Prachka.Application.Abstractions.Querying.Order.IQueryBuilder;
using IOrderSubscriptionQueryBuilder =
    VyazmaTech.Prachka.Application.Abstractions.Querying.OrderSubscription.IQueryBuilder;
using IQueueQueryBuilder = VyazmaTech.Prachka.Application.Abstractions.Querying.Queue.IQueryBuilder;
using IQueueSubscriptionQueryBuilder =
    VyazmaTech.Prachka.Application.Abstractions.Querying.QueueSubscription.IQueryBuilder;
using IUserQueryBuilder = VyazmaTech.Prachka.Application.Abstractions.Querying.User.IQueryBuilder;

namespace VyazmaTech.Prachka.Application.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryBuilders(this IServiceCollection services)
    {
        services.AddSingleton<IOrderQueryBuilder, OrderQuery.QueryBuilder>();
        services.AddSingleton<IQueueQueryBuilder, QueueQuery.QueryBuilder>();
        services.AddSingleton<IUserQueryBuilder, UserQuery.QueryBuilder>();
        services.AddSingleton<IOrderSubscriptionQueryBuilder, OrderSubscriptionQuery.QueryBuilder>();
        services.AddSingleton<IQueueSubscriptionQueryBuilder, QueueSubscriptionQuery.QueryBuilder>();

        return services;
    }

    public static IServiceCollection AddQuerying(this IServiceCollection services)
    {
        services.AddEntityQuery<OrderByQuery.Query, IOrderQueryBuilder>();
        services.AddEntityQuery<UserByQuery.Query, IUserQueryBuilder>();
        services.AddEntityQuery<QueueByQuery.Query, IQueueQueryBuilder>();

        services
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<OrderByQuery.Query, IOrderQueryBuilder>()
            .AddQueryChain<UserByQuery.Query, IUserQueryBuilder>()
            .AddQueryChain<QueueByQuery.Query, IQueueQueryBuilder>();

        return services;
    }

    public static IServiceCollection AddCurrentUsers(this IServiceCollection services)
    {
        services.AddScoped<CurrentUserProxy>();
        services.AddScoped<ICurrentUser>(x => x.GetRequiredService<CurrentUserProxy>());
        services.AddScoped<ICurrentUserManager>(x => x.GetRequiredService<CurrentUserProxy>());

        return services;
    }

    private static void AddEntityQuery<TQuery, TQueryBuilder>(this IServiceCollection services)
    {
        services.AddSingleton<IQueryProcessor<TQuery, TQueryBuilder>, QueryProcessor<TQuery, TQueryBuilder>>();
    }

    private static IChainConfigurator AddQueryChain<TQuery, TQueryBuilder>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<QueryRequest<TQuery, TQueryBuilder>, TQueryBuilder>(
            x => x
                .ThenFromAssemblies(IApplicationMarker.Assembly)
                .FinishWith((r, _) => r.Builder));
    }
}