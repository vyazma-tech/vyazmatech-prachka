using Application.DataAccess.Contracts;
using Application.DataAccess.Contracts.Repositories;
using Domain.Kernel;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Interceptors;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> options)
    {
        services.AddDbContext<DatabaseContext>(options);
        services.AddInfrastructure();
        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> options)
    {
        services.AddDbContext<DatabaseContext>(options);
        services.AddInfrastructure();
        return services;
    }

    public static async Task UseDatabase(this IServiceScope scope)
    {
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await context.Database.MigrateAsync();
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, DatabaseContext>();
        services.AddScoped<IPersistenceContext, PersistenceContext>();
        services.AddSingleton<PublishDomainEventsInterceptor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IQueueRepository, QueueRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderSubscriptionRepository, OrderSubscriptionRepository>();
        services.AddScoped<IQueueSubscriptionRepository, QueueSubscriptionRepository>();
        services.AddTransient<IDateTimeProvider, SpbDateTimeProvider>();

        return services;
    }
}