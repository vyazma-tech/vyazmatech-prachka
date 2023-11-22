using Xunit;

namespace Test.Common.Fixtures.WebFactory;

[CollectionDefinition(nameof(WebAppFactory))]
public class WebAppFactoryCollectionFixture : ICollectionFixture<WebAppFactory>
{
}