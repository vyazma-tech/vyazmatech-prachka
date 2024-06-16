using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Application.Handlers.Tests;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public abstract class TestBase : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _fixture;
    private readonly Func<Task> _reset;

    protected TestBase(CoreDatabaseFixture fixture)
    {
        _fixture = fixture;
        _reset = fixture.ResetAsync;
    }

    protected IPersistenceContext PersistenceContext => _fixture.PersistenceContext;

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