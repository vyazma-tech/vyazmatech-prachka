using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;

public class CoreDatabaseFixture : DatabaseFixture
{
    public DatabaseContext Context { get; private set; } = null!;
    public AsyncServiceScope Scope { get; private set; }
    public IPersistenceContext PersistenceContext { get; private set; } = null!;

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
        services.AddDatabase(x =>
            x.UseNpgsql(Container.GetConnectionString()));
        services.AddInfrastructure();
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
    }
}