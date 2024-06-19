using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Infrastructure.Authentication.Configuration;
using VyazmaTech.Prachka.Infrastructure.Authentication.Interceptors;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models;
using VyazmaTech.Prachka.Infrastructure.Authentication.Services;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<TokenConfiguration>()
            .BindConfiguration(TokenConfiguration.SectionKey);

        PostgresConfiguration postgresConfiguration = configuration
                                                          .GetSection(PostgresConfiguration.SectionKey)
                                                          .Get<PostgresConfiguration>()
                                                      ?? throw new InvalidOperationException(
                                                          "Identity postgres not configured.");

        services.AddScoped<IAuthenticationService, TelegramAuthenticationService>();
        services.AddSingleton<IntegrationEventToOutboxMessageInterceptor>();

        services.AddDbContext<VyazmaTechIdentityContext>(
            (sp, builder) =>
            {
                IntegrationEventToOutboxMessageInterceptor interceptor = sp
                    .GetRequiredService<IntegrationEventToOutboxMessageInterceptor>();

                builder
                    .UseNpgsql(postgresConfiguration.ToConnectionString())
                    .AddInterceptors(interceptor);
            });

        services.AddIdentity<VyazmaTechIdentityUser, VyazmaTechIdentityRole>()
            .AddEntityFrameworkStores<VyazmaTechIdentityContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}