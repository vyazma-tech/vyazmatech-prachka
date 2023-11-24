using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public abstract class TestBase : IAsyncLifetime
{
    private readonly Func<Task> _reset; 
    protected readonly CoreDatabaseFixture Database;

    protected TestBase(CoreDatabaseFixture database)
    {
        Database = database;
        _reset = database.ResetAsync;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _reset();
    }
}