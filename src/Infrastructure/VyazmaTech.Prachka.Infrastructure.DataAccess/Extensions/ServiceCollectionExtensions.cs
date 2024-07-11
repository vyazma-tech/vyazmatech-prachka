using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Interceptors;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;
using VyazmaTech.Prachka.Infrastructure.Tools;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;

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

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, PersistenceContext>();
        services.AddScoped<IPersistenceContext, PersistenceContext>();
        services.AddSingleton<DomainEventsToOutboxMessageInterceptor>();
        services.AddSingleton<AuditableEntityUpdatingInterceptor>();

        services.AddTransient<IDateTimeProvider, DefaultTimeProvider>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IQueueRepository, QueueRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}