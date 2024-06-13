using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Interceptors;
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
        services.AddSingleton<PublishDomainEventsInterceptor>();

        services.AddTransient<IDateTimeProvider, DefaultTimeProvider>();

        return services;
    }
}