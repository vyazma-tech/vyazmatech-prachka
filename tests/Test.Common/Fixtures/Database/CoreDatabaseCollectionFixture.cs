using Xunit;

namespace Test.Common.Fixtures.Database;

[CollectionDefinition(nameof(CoreDatabaseCollectionFixture))]
public class CoreDatabaseCollectionFixture : ICollectionFixture<CoreDatabaseFixture>
{
}