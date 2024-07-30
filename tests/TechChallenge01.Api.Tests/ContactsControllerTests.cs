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
        ContactResponse contactResponse = JsonSerializer.Deserialize<ContactResponse>(content, jsonOptions)!;
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
        var contactResponse = JsonSerializer.Deserialize<List<ContactResponse>>(content, jsonOptions)!;
        contactResponse.Should().NotBeNull();
        contactResponse.Should().HaveCountGreaterThan(0);
    }

    [Fact(DisplayName = "Deve retornar uma lista com filtro de ddd")]
    [Trait("Functional", "ContactsController")]
    public async Task Should_Return_ListWithContactsFiltered()
    {
        //Arrange 
        await Should_Return_ContactCreatedWithSuccess();

        //Act
        var response = await HttpClient.GetAsync($"api/contacts?ddd=15");

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var contactResponse = JsonSerializer.Deserialize<List<ContactResponse>>(content, jsonOptions)!;
        contactResponse.Should().NotBeNull();
        contactResponse.Should().HaveCount(0);
    }


    [Fact(DisplayName = "Deve deletar o contato")]
    [Trait("Functional", "ContactsController")]
    public async Task Should_Delete_Contact()
    {
        //Arrange 

        await Should_Return_ContactCreatedWithSuccess();
        
        //Act
        var requestDelete = await HttpClient.DeleteAsync($"api/contacts/delete?id=1");
        var getContact = await HttpClient.GetAsync($"api/contacts");

        //Assert
        requestDelete.EnsureSuccessStatusCode();
        requestDelete.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await getContact.Content.ReadAsStringAsync();
        var contactResponse = JsonSerializer.Deserialize<List<ContactResponse>>(content)!;
        contactResponse.Should().HaveCount(0);
    
    }
}
