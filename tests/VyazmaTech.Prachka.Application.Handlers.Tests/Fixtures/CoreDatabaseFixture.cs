using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Configuration;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;
using VyazmaTech.Prachka.Infrastructure.Jobs.Configuration;
using VyazmaTech.Prachka.Infrastructure.Jobs.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;

public class CoreDatabaseFixture : DatabaseFixture
{
    public DatabaseContext Context { get; private set; } = null!;

    public AsyncServiceScope Scope { get; private set; }

    public IPersistenceContext PersistenceContext { get; private set; } = null!;

    public IUnitOfWork UnitOfWork { get; private set; } = null!;

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
        var configuration = ReplaceJobsConfiguration();

        services
            .AddDatabase(
                x =>
                    x.UseNpgsql(Container.GetConnectionString())
                        .EnableSensitiveDataLogging()
                        .LogTo(Console.WriteLine));

        services
            .AddInfrastructure()
            .AddJobs(configuration);
    }

    private void ReplacePostgresConfiguration(IServiceCollection services)
    {
        var postgresConfiguration = new PostgresConfiguration
        {
            ConnectionString = Container.GetConnectionString()
        };

        services.AddSingleton(postgresConfiguration);
    }

    private IConfiguration ReplaceJobsConfiguration()
    {
        var configuration = new ConfigurationBuilder();

        var collection = new Dictionary<string, string?>
        {
            [nameof(SchedulingConfiguration.SeedingInterval)] = 10.ToString(),
            [nameof(SchedulingConfiguration.DefaultCapacity)] = 10.ToString(),
            [nameof(SchedulingConfiguration.WeekdayActiveFrom)] = "10:00:00",
            [nameof(SchedulingConfiguration.WeekdayActiveUntil)] = "17:00:00",
            [nameof(SchedulingConfiguration.DayOfActiveFrom)] = "10:00:00",
            [nameof(SchedulingConfiguration.DayOfActiveUntil)] = "17:00:00",
        };

        configuration.AddInMemoryCollection(collection);

        return configuration.Build();
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
        PersistenceContext = Scope.ServiceProvider.GetRequiredService<IPersistenceContext>();
        UnitOfWork = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }
}