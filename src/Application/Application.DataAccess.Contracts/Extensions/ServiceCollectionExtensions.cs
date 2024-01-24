using Application.DataAccess.Contracts.Querying.Order;
using Application.DataAccess.Contracts.Querying.OrderSubscription;
using Application.DataAccess.Contracts.Querying.Queue;
using Application.DataAccess.Contracts.Querying.QueueSubscription;
using Application.DataAccess.Contracts.Querying.User;
using Microsoft.Extensions.DependencyInjection;
using IQueryBuilder = Application.DataAccess.Contracts.Querying.Order.IQueryBuilder;

namespace Application.DataAccess.Contracts.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryBuilders(this IServiceCollection services)
    {
        services.AddSingleton<IQueryBuilder, OrderQuery.QueryBuilder>();
        services.AddSingleton<Querying.Queue.IQueryBuilder, QueueQuery.QueryBuilder>();
        services.AddSingleton<Querying.User.IQueryBuilder, UserQuery.QueryBuilder>();
        services.AddSingleton<Querying.OrderSubscription.IQueryBuilder, OrderSubscriptionQuery.QueryBuilder>();
        services.AddSingleton<Querying.QueueSubscription.IQueryBuilder, QueueSubscriptionQuery.QueryBuilder>();

        return services;
    }
}