using Application.DataAccess.Contracts;
using Infrastructure.DataAccess.Contexts;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public abstract class TestBase : IAsyncLifetime
{
    private readonly Func<Task> _reset;
    protected readonly CoreDatabaseFixture Fixture;

    protected TestBase(CoreDatabaseFixture fixture)
    {
        Fixture = fixture;
        _reset = fixture.ResetAsync;
    }

    protected IPersistenceContext PersistenceContext => Fixture.PersistenceContext;
    protected DatabaseContext Context => Fixture.Context;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _reset();
    }
}