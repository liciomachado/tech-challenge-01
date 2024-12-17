using MassTransit;
using Moq;
using System.Text.Json;
using TechChallenge01.Application.Consumers;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.UseCases;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.Tests
{
    public class ContactsV2ControllerTests
    {
        private readonly Mock<IContactPublisher> _contactPublisherMock;
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly Mock<IServiceProvider> _serviceProvider;

        private readonly InsertContactConsumer _insertContactConsumer;
        private readonly UpdateContactConsumer _updateContactConsumer;
        private readonly DeleteContactConsumer _deleteContactConsumer;

        private readonly InsertContactUseCaseV2 _insertConcatUseCase;
        private readonly DeleteContactUseCaseV2 _deleteConcatUseCase;
        private readonly UpdateContactUseCaseV2 _updateConcatUseCase;

        public ContactsV2ControllerTests()
        {
            _contactPublisherMock = new Mock<IContactPublisher>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _serviceProvider = new Mock<IServiceProvider>();
            _insertConcatUseCase = new InsertContactUseCaseV2(_contactPublisherMock.Object);
            _deleteConcatUseCase = new DeleteContactUseCaseV2(_contactPublisherMock.Object, _contactRepositoryMock.Object);
            _updateConcatUseCase = new UpdateContactUseCaseV2(_contactPublisherMock.Object, _contactRepositoryMock.Object);
            _insertContactConsumer = new InsertContactConsumer(_serviceProvider.Object);
            _updateContactConsumer = new UpdateContactConsumer(_serviceProvider.Object);
            _deleteContactConsumer = new DeleteContactConsumer(_serviceProvider.Object);
        }

        [Fact(DisplayName = "Deve consumir a mensagem do contato criado")]
        public async Task Should_Consume_Message_When_Published()
        {
            var insertContactRequest = new InsertContactRequest("João Silva", "11-99999-9999", "email@valido.com");
            var insertContactEvent = new InsertContactEvent
            {
                Name = insertContactRequest.Name,
                PhoneNumber = insertContactRequest.PhoneNumber,
                Email = insertContactRequest.Email
            };

            _contactPublisherMock.Setup(repo => repo.PublishInsertContactAsync(It.IsAny<InsertContactEvent>()))
                                 .Returns(Task.CompletedTask);

            var consumerMock = new Mock<ConsumeContext<InsertContactEvent>>();
            consumerMock.Setup(c => c.Message).Returns(insertContactEvent);

            await _insertConcatUseCase.Execute(insertContactRequest);
            await _insertContactConsumer.Consume(consumerMock.Object);

            _contactPublisherMock.Verify(repo => repo.PublishInsertContactAsync(It.IsAny<InsertContactEvent>()), Times.Once);
            consumerMock.Verify(c => c.Message, Times.Once);
        }

        
        [Fact(DisplayName = "Deve consumir a mensagem de atualização de contato")]
        public async Task Should_Consume_Update_Message_When_Published()
        {
            var contact = new Contact { Id = 1, Name = "Contato Atualizado", Email = "atualizado@example.com", PhoneNumber = new PhoneNumber("11333333333") };
            var updateContactEvent = new UpdateContactEvent
            {
                Id = contact.Id,
                Name = contact.Name,
                PhoneNumber = contact.PhoneNumber.Value,
                Email = contact.Email
            };

            _contactRepositoryMock.Setup(service => service.GetByIdAsync(1))
                                   .ReturnsAsync(contact);

            _contactPublisherMock.Setup(repo => repo.PublishUpdateContactAsync(It.IsAny<UpdateContactEvent>()))
                                 .Returns(Task.CompletedTask);

            var consumerMock = new Mock<ConsumeContext<UpdateContactEvent>>();
            consumerMock.Setup(c => c.Message).Returns(updateContactEvent);

            var updateRequest = new UpdateContactRequest(((int)contact.Id), contact.Name, contact.PhoneNumber.Value, contact.Email);
            await _updateConcatUseCase.Execute(updateRequest);
            await _updateContactConsumer.Consume(consumerMock.Object);

            _contactPublisherMock.Verify(repo => repo.PublishUpdateContactAsync(It.IsAny<UpdateContactEvent>()), Times.Once);
            consumerMock.Verify(c => c.Message, Times.Once);
        }

        [Fact(DisplayName = "Deve consumir a mensagem de exclusão de contato")]
        public async Task Should_Consume_Delete_Message_When_Published()
        {
            var contact = new Contact { Id = 1, Name = "Contato Removido", Email = "removido@example.com", PhoneNumber = new PhoneNumber("11333333333") };
            var deleteContactEvent = new DeleteContactEvent
            {
                Id = contact.Id
            };

            _contactRepositoryMock.Setup(service => service.GetByIdAsync(1))
                                   .ReturnsAsync(contact);
            _contactPublisherMock.Setup(repo => repo.PublishDeleteContactAsync(It.IsAny<DeleteContactEvent>()))
                                 .Returns(Task.CompletedTask);

            var consumerMock = new Mock<ConsumeContext<DeleteContactEvent>>();
            consumerMock.Setup(c => c.Message).Returns(deleteContactEvent);

            await _deleteConcatUseCase.Delete(contact.Id);
            await _deleteContactConsumer.Consume(consumerMock.Object);

            _contactPublisherMock.Verify(repo => repo.PublishDeleteContactAsync(It.IsAny<DeleteContactEvent>()), Times.Once);
            consumerMock.Verify(c => c.Message, Times.Once);
        }

        [Fact(DisplayName = "Deve atualizar um contato com sucesso")]
        public async Task Should_Return_Contact_Update_ShouldWithSuccess()
        {
            var contact = new Contact { Id = 1, Name = "Contato Atualizado", Email = "atualizado@example.com", PhoneNumber = new PhoneNumber("11333333333") };
            _contactRepositoryMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(contact);
            _contactRepositoryMock.Setup(service => service.Update(contact));

            var updateRequest = new UpdateContactRequest(((int)contact.Id), contact.Name, contact.PhoneNumber.Value, contact.Email);
            var updateEvent = new UpdateContactEvent(((int)contact.Id), contact.Name, contact.PhoneNumber.Value, contact.Email);

            var result = await _updateConcatUseCase.Execute(updateRequest);
            var data = JsonSerializer.Deserialize<UpdateContactRequest>(JsonSerializer.Serialize(result.Data));

            var consumerMock = new Mock<ConsumeContext<UpdateContactEvent>>();
            consumerMock.Setup(c => c.Message).Returns(updateEvent);

            await _updateContactConsumer.Consume(consumerMock.Object);

            _contactPublisherMock.Verify(repo => repo.PublishUpdateContactAsync(It.IsAny<UpdateContactEvent>()), Times.Once);
            consumerMock.Verify(c => c.Message, Times.Once);

            Assert.Equal(result.Message, "Atualização em processamento.");
            Assert.Equal(data.Name, updateRequest.Name);
            Assert.Equal(data.Email, updateRequest.Email);
        }

        [Fact(DisplayName = "Deve remover um contato com sucesso")]
        public async Task Delete_ShouldRemoveContact()
        {
            var contact = new Contact { Id = 1, Name = "Contato Atualizado", Email = "atualizado@example.com", PhoneNumber = new PhoneNumber("11333333333") };
            
            _contactRepositoryMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(contact);
            _contactRepositoryMock.Setup(service => service.Delete(contact));

            var deleteEvent = new DeleteContactEvent
            {
                Id = contact.Id
            };

            var consumerMock = new Mock<ConsumeContext<DeleteContactEvent>>();
            consumerMock.Setup(c => c.Message).Returns(deleteEvent);
            
            await _deleteContactConsumer.Consume(consumerMock.Object);
            PublishResponse response = await _deleteConcatUseCase.Delete(contact.Id);
            
            var responseMessage = JsonSerializer.Deserialize<DeleteContactEvent>(JsonSerializer.Serialize(response.Data));

            _contactPublisherMock.Verify(p => p.PublishDeleteContactAsync(It.IsAny<DeleteContactEvent>()), Times.Once);
            consumerMock.Verify(c => c.Message, Times.Once);
            Assert.Equal(response.Message, "Exclusão em processamento.");
            Assert.Equal(responseMessage.Id, contact.Id);
        }
    }
}