using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public abstract class TestBase : IAsyncLifetime
{
    protected readonly CoreDatabaseFixture Fixture;
    private readonly Func<Task> _reset;

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