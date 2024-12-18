using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using TechChallenge01.ApiQueue.Tests.Abstractions;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.ApiQueue.Tests
{
    public class ContactsControllerTests : BaseFunctionalTests
    {
        private readonly Faker _faker;
        private readonly FunctionalTestWebAppFactory _testsFixture;

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ContactsControllerTests(FunctionalTestWebAppFactory testsFixture) : base(testsFixture)
        {
            _testsFixture = testsFixture;
            _faker = new Faker(locale: "pt_BR");
        }

        private string validPhoneNumber => "(11) 99999-9999";
        private string validPhoneNumberToUpdate => "(12) 98888-8888";

        [Fact(DisplayName = "Deve inserir um contato com sucesso")]
        [Trait("Functional", "ContactsController")]
        public async Task Should_Return_ContactCreatedWithSuccess()
        {
            //Arrange
            InsertContactRequest insertContactRequest = new(_faker.Name.FullName(), validPhoneNumber, _faker.Internet.Email());

            //Act
            var response = await HttpClient.PostAsJsonAsync($"api/contacts", insertContactRequest);

            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            PublishResponse contactResponse = JsonSerializer.Deserialize<PublishResponse>(content, jsonOptions)!;
            contactResponse.Should().NotBeNull();
            contactResponse.Data.Name.Should().BeEquivalentTo(insertContactRequest.Name);
            var formatedPhone = Regex.Replace(insertContactRequest.PhoneNumber, "[^0-9]+", "");
            contactResponse.Data.PhoneNumber.Should().BeEquivalentTo(formatedPhone);
            contactResponse.Data.Email.Should().BeEquivalentTo(insertContactRequest.Email);
        }

       
 

        [Fact(DisplayName = "Deve atualizar um contato com sucesso")]
        [Trait("Functional", "ContactsController")]
        public async Task Should_Return_ContactUpdatedWithSuccess()
        {
            //Arrange
            UpdateContactRequest updateContactRequest = new(1, _faker.Name.FullName(), validPhoneNumberToUpdate, _faker.Internet.Email());

            //Act
            var response = await HttpClient.PutAsJsonAsync($"api/contacts", updateContactRequest);

            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            PublishResponse contactResponse = JsonSerializer.Deserialize<PublishResponse>(content, jsonOptions)!;
            contactResponse.Should().NotBeNull();
            contactResponse.Data.Name.Should().BeEquivalentTo(updateContactRequest.Name);
            var formatedPhone = Regex.Replace(updateContactRequest.PhoneNumber, "[^0-9]+", "");
            contactResponse.Data.PhoneNumber.Should().BeEquivalentTo(formatedPhone);
            contactResponse.Data.Email.Should().BeEquivalentTo(updateContactRequest.Email);
        }

        [Fact(DisplayName = "Deve retornar erro ao tentar atualizar um contato inexistente")]
        [Trait("Functional", "ContactsController")]
        public async Task Should_Return_ErrorToUpdateContactWhenNotExists()
        {
            //Arrange
            UpdateContactRequest updateContactRequest = new(999, _faker.Name.FullName(), validPhoneNumberToUpdate, _faker.Internet.Email());

            //Act
            var response = await HttpClient.PutAsJsonAsync($"api/contacts", updateContactRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            ErrorMessageResponse badRequestError = JsonSerializer.Deserialize<ErrorMessageResponse>(content, jsonOptions)!;
            badRequestError.Should().NotBeNull();
            badRequestError.Message.Should().Be("Não foi possível localizar o cadastro do contato informado.");
        }

        [Theory(DisplayName = "Deve retornar erro ao tentar atualizar um contato com um e-mail inválido")]
        [Trait("Functional", "ContactsController")]
        [InlineData("john.emailinvalido.com", "Este endereço de e-mail não é válido.")]
        [InlineData($"emailcommaisdoquecinquentacaracteres@dominiomaiordoqueesperado.com", "Tamanho inválido, máximo de 50 caracteres.")]
        public async Task Should_Return_ErrorToUpdateContactWhenEmailIsInvalid(string email, string expectedErrorMessage)
        {
            //Arrange
            UpdateContactRequest updateContactRequest = new(1, _faker.Name.FullName(), validPhoneNumberToUpdate, email);

            //Act
            var response = await HttpClient.PutAsJsonAsync($"api/contacts", updateContactRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            ValidationProblemDetails validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, jsonOptions)!;
            validationProblemDetails.Should().NotBeNull();
            validationProblemDetails.Errors["Email"].Should().Contain(expectedErrorMessage);
        }

        [Fact(DisplayName = "Deve retornar erro ao tentar atualizar um contato com um e-mail com tamanho inválido")]
        [Trait("Functional", "ContactsController")]
        public async Task Should_Return_ErrorToUpdateContactWhenEmailLengthIsInvalid()
        {
            //Arrange
            UpdateContactRequest updateContactRequest = new(1, _faker.Name.FullName(), validPhoneNumberToUpdate, $"{_faker.Lorem.Letter(39)}@dominio.com");

            //Act
            var response = await HttpClient.PutAsJsonAsync($"api/contacts", updateContactRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            ValidationProblemDetails validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, jsonOptions)!;
            validationProblemDetails.Should().NotBeNull();
            validationProblemDetails.Errors["Email"].Should().Contain("Tamanho inválido, máximo de 50 caracteres.");
        }

        [Fact(DisplayName = "Deve retornar erro ao tentar atualizar um contato com um nome com tamanho inválido")]
        [Trait("Functional", "ContactsController")]
        public async Task Should_Return_ErrorToUpdateContactWhenNameLengthIsInvalid()
        {
            //Arrange
            UpdateContactRequest updateContactRequest = new(1, _faker.Lorem.Letter(256), validPhoneNumberToUpdate, _faker.Internet.Email());

            //Act
            var response = await HttpClient.PutAsJsonAsync($"api/contacts", updateContactRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            ValidationProblemDetails validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, jsonOptions)!;
            validationProblemDetails.Should().NotBeNull();
            validationProblemDetails.Errors["Name"].Should().Contain("Tamanho inválido, máximo de 255 caracteres.");
        }

        [Theory(DisplayName = "Deve retornar erro ao tentar atualizar um contato com um número de telefone com tamanho inválido")]
        [Trait("Functional", "ContactsController")]
        [InlineData("113735555")] // Menor do que esperado
        [InlineData("(11) 3567898888-8888856473")] // Maior do que esperado
        public async Task Should_Return_ErrorToUpdateContactWhenPhoneNumberLengthIsInvalid(string number)
        {
            //Arrange
            await Should_Return_ContactCreatedWithSuccess();
            var getContacts = await HttpClient.GetFromJsonAsync<List<ContactResponse>>($"api/contacts/GetAll");
            var id = getContacts!.First().Id;
            UpdateContactRequest updateContactRequest = new(id, _faker.Name.FullName(), number, _faker.Internet.Email());

            //Act
            var response = await HttpClient.PutAsJsonAsync($"api/contacts", updateContactRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            ValidationProblemDetails validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, jsonOptions)!;
            validationProblemDetails.Should().NotBeNull();
            validationProblemDetails.Errors["PhoneNumber"].Should().Contain("Tamanho inválido, deve ter entre 10 e 20 caracteres.");
        }

        [Fact(DisplayName = "Deve deletar o contato")]
        [Trait("Functional", "ContactsController")]
        public async Task Should_Delete_Contact()
        {
            //Arrange 
            await Should_Return_ContactCreatedWithSuccess();
            var getContacts = await HttpClient.GetFromJsonAsync<List<ContactResponse>>($"api/contacts/GetAll");
            var id = getContacts!.First().Id;
            //Act
            var requestDelete = await HttpClient.DeleteAsync($"api/contacts/delete?id={id}");

            //Assert
            requestDelete.EnsureSuccessStatusCode();
            requestDelete.StatusCode.Should().Be(HttpStatusCode.OK);
        }



    }
}
