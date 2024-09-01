namespace Aram.BFF.Api.IntegrationTests.Common;

[CollectionDefinition(CollectionName)]
public class ApplicationApiFactoryCollection : ICollectionFixture<ApplicationApiFactory>
{
    private const string CollectionName = "ApplicationApiFactoryCollection";
}