using Xunit;

namespace Test.Handlers.Fixtures;

[CollectionDefinition(nameof(CoreDatabaseCollectionFixture))]
public class CoreDatabaseCollectionFixture : ICollectionFixture<CoreDatabaseFixture>
{
}