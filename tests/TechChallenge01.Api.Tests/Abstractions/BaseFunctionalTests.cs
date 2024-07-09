namespace TechChallenge01.Api.Tests.Abstractions;

public class BaseFunctionalTests : IClassFixture<FunctionalTestWebAppFactory>
{
    protected HttpClient HttpClient { get; init; }

    public BaseFunctionalTests(FunctionalTestWebAppFactory webAppFactory)
    {
        HttpClient = webAppFactory.CreateClient();
    }
}
