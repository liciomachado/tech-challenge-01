using Bogus;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TechChallenge01.Api.Tests.Abstractions;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Api.Tests
{
    public class TokenControllerTests : BaseFunctionalTests
    {
        private readonly Faker _faker;
        private readonly FunctionalTestWebAppFactory _testsFixture;

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public TokenControllerTests(FunctionalTestWebAppFactory testsFixture) : base(testsFixture)
        {
            _testsFixture = testsFixture;
            _faker = new Faker(locale: "pt_BR");
        }

        [Fact(DisplayName = "Deve gerar Token Válido")]
        [Trait("Functional", "TokenController")]
        public async Task Should_Return_TokenCreatedWithSuccess()
        {
            //Arrange
            var usuario = new UserRequest { Username = "admin", Password = "admin@123" };

            //Act
            var response = await HttpClient.PostAsJsonAsync($"api/token", usuario);

            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Deve gerar erro de não autorizado")]
        [Trait("Functional", "TokenController")]
        public async Task Should_Return_ErrorTokenUnauthorized()
        {
            //Arrange
            var usuario = new UserRequest { Username = "admin", Password = "senhainvalida" };

            //Act
            var response = await HttpClient.PostAsJsonAsync($"api/token", usuario);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
