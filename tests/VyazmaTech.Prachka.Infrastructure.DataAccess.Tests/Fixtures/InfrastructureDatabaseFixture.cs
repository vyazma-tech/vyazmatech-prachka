using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Configuration;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;
using VyazmaTech.Prachka.Tests.Tools.Fixtures;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Tests.Fixtures;

public class InfrastructureDatabaseFixture : DatabaseFixture
{
    public DatabaseContext Context { get; private set; } = null!;

    public AsyncServiceScope Scope { get; private set; }

    public override async Task ResetAsync()
    {
        await base.ResetAsync();
        Context.ChangeTracker.Clear();
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await Scope.DisposeAsync();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        ReplacePostgresConfiguration(services);

        services
            .AddDatabase(
                x =>
                    x.UseNpgsql(Container.GetConnectionString())
                        .EnableSensitiveDataLogging()
                        .LogTo(Console.WriteLine));

        services
            .AddInfrastructure();
    }

    private void ReplacePostgresConfiguration(IServiceCollection services)
    {
        var postgresConfiguration = new PostgresConfiguration
        {
            ConnectionString = Container.GetConnectionString()
        };

        services.AddSingleton(postgresConfiguration);
    }

    protected override DbConnection CreateConnection()
    {
        return Context.Database.GetDbConnection();
    }

    protected override async Task UseProviderAsync(IServiceProvider provider)
    {
        Scope = provider.CreateAsyncScope();
        await Scope.UseDatabase();

        Context = Scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    }
}