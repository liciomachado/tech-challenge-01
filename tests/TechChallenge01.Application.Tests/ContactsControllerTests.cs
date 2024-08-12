using Moq;
using TechChallenge01.Application.UseCases;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Core;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.Tests
{
    public class ContactsControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly InsertContactUseCase _insertConcatUseCase;
        private readonly DeleteContactUseCase _deleteConcatUseCase;
        private readonly UpdateContactUseCase _updateConcatUseCase;
        private readonly GetContactsUseCase _getConcatUseCase;
        public ContactsControllerTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _contactRepositoryMock.Setup(uow => uow.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _insertConcatUseCase = new InsertContactUseCase(_contactRepositoryMock.Object);
            _getConcatUseCase = new GetContactsUseCase(_contactRepositoryMock.Object);
            _deleteConcatUseCase = new DeleteContactUseCase(_contactRepositoryMock.Object);
            _updateConcatUseCase = new UpdateContactUseCase(_contactRepositoryMock.Object);
        }


        [Fact(DisplayName = "Deve inserir um contato com sucesso")]
        public async Task Should_Return_ContactCreatedWithSuccess()
        {
            //Arrange            


            InsertContactRequest insertContactRequest = new("João Silva", "11-99999-9999", "email@valido.com");

            //Act
            await _insertConcatUseCase.Execute(insertContactRequest);

            //Assert
            _contactRepositoryMock.Verify(r => r.Save(It.IsAny<Contact>()), Times.Once);
        }

        [Theory(DisplayName = "Deve retornar uma  exceção de um  contato inválido")]
        [InlineData("", "1234567890", "joao.@example.com", "O nome é obrigatório.")]
        [InlineData("João Silva", "123", "joao.silva@example.com", "Número de telefone informado incorretamente, Modelo esperado: (dd) 99999-9999.")]
        [InlineData("João Silva", "1234567890", "invalid-email", "Formato de e-mail inválido.")]
        public async Task Should_Return_ContactInvalidWith_ThrowsArgumentException(string Name, string phoneNumber, string email, string expectedErrorMessage)
        {
            //Arrange 

            InsertContactRequest insertContactRequest = new(Name, phoneNumber, email);

            //Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _insertConcatUseCase.Execute(insertContactRequest));

            //Assert
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact(DisplayName = "Deve retornar um contato filtrado por ddd")]
        public async Task Should_Return_ContactFilteredByDDD()
        {
            //Arrange 

            var contacts = new List<Contact>
            {
                new Contact( "João Silva", new PhoneNumber("11-99999-9999"),  "john.doe@example.com"  )
            };
            //Act
            _contactRepositoryMock.Setup(r => r.GetByDDD(It.IsAny<string?>())).ReturnsAsync(contacts);

            var result = await _getConcatUseCase.Execute("11");

            //Assert
            var mapped = result.Select(x => new ContactResponse(x.Id, x.Name, x.PhoneNumber, x.Email, x.ddd)).ToList();
            Assert.Equal(mapped, result);
        }


        [Fact(DisplayName = "Deve atualizar um contato com sucesso")]
        public async Task Should_Return_Contact_Update_ShouldWithSuccess()
        {

            var Contact = new Contact { Id = 1, Name = "Contato Atualizado", Email = "atualizado@example.com", PhoneNumber = new PhoneNumber("11333333333") };
            var updateRequest = new UpdateContactRequest(((int)Contact.Id), Contact.Name, Contact.PhoneNumber.Value, Contact.Email);
            _contactRepositoryMock.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync(Contact);
            _contactRepositoryMock.Setup(service => service.Update(Contact));


            var result = await _updateConcatUseCase.Execute(updateRequest);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Deve remover um contato com sucesso")]
        public async Task Delete_ShouldRemoveContact()
        {

            // Arrange
            var Contact = new Contact { Id = 1, Name = "Contato Atualizado", Email = "atualizado@example.com", PhoneNumber = new PhoneNumber("11333333333") };
            _contactRepositoryMock.Setup(service => service.GetByIdAsync(1))
                          .ReturnsAsync(Contact);
            _contactRepositoryMock.Setup(service => service.Delete(Contact));

            // Act
            await _deleteConcatUseCase.Delete(Contact.Id);

            // Assert
            _contactRepositoryMock.Verify(repo => repo.Delete(Contact), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);

        }


        [Fact(DisplayName = "Deve retornar um contato filtrado por Id")]
        public void GetById_ShouldReturnContact_WhenIdExists()
        {
            // Arrange
            var contact = new Contact("Teste", new PhoneNumber("1234567890"), "test@example.com");
            contact.Id = 1;
            _contactRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(contact);

            // Act
            var result = _getConcatUseCase.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Teste", result.Result.Name);
        }


        [Fact(DisplayName = "Deve retornar uma lista com todos os contatos")]
        public async Task GetAll_ShouldReturnAllContacts()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, Name = "teste 1", Email = "teste1@example.com", PhoneNumber = new PhoneNumber( "1234567890" )},
                new Contact { Id = 2, Name = "TESTE 2", Email = "teste2@example.com", PhoneNumber = new PhoneNumber( "1187654321") }
            };
            _contactRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(contacts);

            // Act
            var result = await _getConcatUseCase.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _contactRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }
    }
}