using Application.Core.Contracts.Orders.Queries;
using Application.Core.Contracts.Queues.Queries;
using Application.Core.Contracts.Users.Queries;
using Application.Core.Querying.Common;
using FluentChaining;
using FluentChaining.Configurators;
using Microsoft.Extensions.DependencyInjection;
using IOrderQueryBuilder = Application.DataAccess.Contracts.Querying.Order.IQueryBuilder;
using IQueueQueryBuilder = Application.DataAccess.Contracts.Querying.Queue.IQueryBuilder;
using IUserQueryBuilder = Application.DataAccess.Contracts.Querying.User.IQueryBuilder;

namespace Application.Core.Extensions;

public static class ServiceCollectionExtensions
{
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

    private static void AddEntityQuery<TQuery, TQueryBuilder>(this IServiceCollection services)
    {
        services.AddSingleton<IQueryProcessor<TQuery, TQueryBuilder>, QueryProcessor<TQuery, TQueryBuilder>>();
    }

    private static IChainConfigurator AddQueryChain<TQuery, TQueryBuilder>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<QueryRequest<TQuery, TQueryBuilder>, TQueryBuilder>(x => x
            .ThenFromAssemblies(IApplicationMarker.Assembly)
            .FinishWith((r, _) => r.Builder));
    }
}