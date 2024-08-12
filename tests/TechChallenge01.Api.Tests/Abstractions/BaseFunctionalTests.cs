using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Api.Tests.Abstractions;

public class BaseFunctionalTests : IClassFixture<FunctionalTestWebAppFactory>
{
    protected HttpClient HttpClient { get; init; }

    public BaseFunctionalTests(FunctionalTestWebAppFactory webAppFactory)
    {
        HttpClient = webAppFactory.CreateClient();
        Login().GetAwaiter().GetResult();
    }

    public async Task Login()
    {
        var usuario = new UserRequest { Username = "admin", Password = "admin@123" };
        var responseToken = await HttpClient.PostAsJsonAsync($"api/token", usuario); // Retorna token JWT Bearer
        responseToken.EnsureSuccessStatusCode();
        var tokenContent = await responseToken.Content.ReadAsStringAsync();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContent);
    }

}
