using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Tests.Fixtures;

[Collection(nameof(InfrastructureDatabaseCollectionFixture))]
public abstract class TestBase : IAsyncLifetime
{
    private readonly InfrastructureDatabaseFixture _fixture;
    private readonly Func<Task> _reset;

    protected TestBase(InfrastructureDatabaseFixture fixture)
    {
        _fixture = fixture;
        _reset = fixture.ResetAsync;
    }

    protected DatabaseContext Context => _fixture.Context;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _reset();
    }
}