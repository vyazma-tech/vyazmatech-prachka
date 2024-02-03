using Application.DataAccess.Contracts;
using Infrastructure.Authentication.Configuration;
using Infrastructure.Authentication.Models;
using Infrastructure.Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<TokenConfiguration>()
            .BindConfiguration(TokenConfiguration.SectionKey);

        IdentityPostgresConnectionConfiguration identityPostgresConfiguration = configuration
            .GetSection(IdentityPostgresConnectionConfiguration.SectionKey)
            .Get<IdentityPostgresConnectionConfiguration>()
            ?? throw new InvalidOperationException("Identity postgres not configured.");

        services.AddScoped<IAuthenticationService, TelegramAuthenticationService>();

        services.AddDbContext<VyazmaTechIdentityContext>(builder => builder
            .UseNpgsql(identityPostgresConfiguration.ToConnectionString()));

        services.AddIdentity<VyazmaTechIdentityUser, VyazmaTechIdentityRole>()
            .AddEntityFrameworkStores<VyazmaTechIdentityContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}