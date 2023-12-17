using Xunit;

namespace Test.Endpoints.Fixtures.WebFactory;

[CollectionDefinition(nameof(WebAppFactoryCollectionFixture))]
public class WebAppFactoryCollectionFixture : ICollectionFixture<WebAppFactory>
{
}