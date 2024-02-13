using System.Data;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures.Extensions;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;

public abstract class DatabaseFixture : IAsyncLifetime
{
    private const string User = "postgres";
    private const string Password = "postgres";
    private const string Database = "postgres";
    private Respawner _respawner;

    protected DatabaseFixture()
    {
        Container = new PostgreSqlBuilder()
            .WithUsername(User)
            .WithPassword(Password)
            .WithDatabase(Database)
            .Build();

        Connection = null!;
        Provider = null!;
        _respawner = null!;
    }

    public PostgreSqlContainer Container { get; }
    public DbConnection Connection { get; private set; }
    protected ServiceProvider Provider { get; private set; }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();

        var services = new ServiceCollection();
        ConfigureServices(services);

        Provider = services.BuildServiceProvider();
        await UseProviderAsync(Provider);

        Connection = CreateConnection();
        bool opened = await Connection.TryOpenAsync(default);

        await InitializeRespawnerAsync();

        if (opened)
        {
            await Connection.CloseAsync();
        }
    }

    public virtual async Task DisposeAsync()
    {
        await Connection.DisposeAsync();
        await Container.DisposeAsync();
        await Provider.DisposeAsync();
    }

    public virtual async Task ResetAsync()
    {
        bool wasOpen = Connection.State is ConnectionState.Open;

        if (wasOpen is false)
        {
            await Connection.OpenAsync();
        }

        await _respawner.ResetAsync(Connection);

        if (wasOpen is false)
        {
            await Connection.CloseAsync();
        }
    }

    protected virtual void ConfigureServices(IServiceCollection services) { }

    protected virtual DbConnection CreateConnection()
    {
        return new NpgsqlConnection(Container.GetConnectionString());
    }

    protected virtual Task UseProviderAsync(IServiceProvider provider)
    {
        return Task.CompletedTask;
    }
    
    private async Task InitializeRespawnerAsync()
    {
        _respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToExclude = new[] { "__EFMigrationsHistory" },
            SchemasToInclude = new[] { "public" },
        });
    }
}