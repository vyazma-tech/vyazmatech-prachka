using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests;

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