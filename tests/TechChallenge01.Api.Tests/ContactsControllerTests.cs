using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TechChallenge01.Api.Tests.Abstractions;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Api.Tests;

public class ContactsControllerTests : BaseFunctionalTests
{
    private readonly FunctionalTestWebAppFactory _testsFixture;

    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public ContactsControllerTests(FunctionalTestWebAppFactory testsFixture) : base(testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Deve inserir um contato com sucesso")]
    [Trait("Functional", "ContactsController")]
    public async Task Should_Return_ContactCreatedWithSuccess()
    {
        //Arrange
        InsertContactRequest insertContactRequest = new("joao", "011-99999-9999", "email@valido.com");

        //Act
        var response = await HttpClient.PostAsJsonAsync($"api/contacts", insertContactRequest);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        InsertContactResponse contactResponse = JsonSerializer.Deserialize<InsertContactResponse>(content, jsonOptions)!;
        contactResponse.Should().NotBeNull();
        contactResponse.Nome.Should().BeEquivalentTo(insertContactRequest.Nome);
        contactResponse.PhoneNumber.Should().BeEquivalentTo(insertContactRequest.PhoneNumber);
        contactResponse.Email.Should().BeEquivalentTo(insertContactRequest.Email);
    }

    [Fact(DisplayName = "Deve retornar uma lista com todos os contatos")]
    [Trait("Functional", "ContactsController")]
    public async Task Should_Return_ListWithAllContacts()
    {
        //Arrange 
        await Should_Return_ContactCreatedWithSuccess();

        //Act
        var response = await HttpClient.GetAsync($"api/contacts");

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var contactResponse = JsonSerializer.Deserialize<List<InsertContactResponse>>(content, jsonOptions)!;
        contactResponse.Should().NotBeNull();
        contactResponse.Should().HaveCountGreaterThan(0);
    }
}
