using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Validations;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.UseCases;

public class InsertContactUseCaseV2([FromServices] IContactPublisher contactPublisher /* Serviço dedicado para publicar */) : IInsertContactUseCase
{
    public async Task<ContactResponse> Execute(InsertContactRequest insertContactRequest)
    {
        if (!ContactValidator.IsValidName(insertContactRequest.Name))
            throw new ArgumentException("O nome é obrigatório.");

        if (!ContactValidator.IsValidEmail(insertContactRequest.Email))
            throw new ArgumentException($"Formato de e-mail inválido.{insertContactRequest.Email}");

        var phoneNumber = new PhoneNumber(insertContactRequest.PhoneNumber);

        var contact = new Contact(insertContactRequest.Name, phoneNumber, insertContactRequest.Email);

        // Cria a mensagem e publica na fila
        await contactPublisher.PublishInsertContacttAsync(new InsertContactEvent
        {
            Name = insertContactRequest.Name,
            Email = insertContactRequest.Email,
            PhoneNumber = insertContactRequest.PhoneNumber
        });

        // TODO: Verificar mensagem de retorno para api
        return new ContactResponse(contact.Id, contact.Name, contact.PhoneNumber.Value, contact.Email, contact.PhoneNumber.DDD);
    }
}


