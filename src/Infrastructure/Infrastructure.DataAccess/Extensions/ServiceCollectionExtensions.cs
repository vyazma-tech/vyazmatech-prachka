using Application.Core.Configuration;
using Application.DataAccess.Contracts;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscription;
using Domain.Core.User;
using Domain.Kernel;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Interceptors;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> options)
    {
        services.AddDbContext<DatabaseContext>(options);
        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> options)
    {
        services.AddDbContext<DatabaseContext>(options);
        return services;
    }

    public static async Task UseDatabase(this IServiceScope scope)
    {
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await context.Database.MigrateAsync();
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, DatabaseContext>();
        services.AddScoped<IPersistenceContext, PersistenceContext>();
        services.AddSingleton<PublishDomainEventsInterceptor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IQueueRepository, QueueRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}