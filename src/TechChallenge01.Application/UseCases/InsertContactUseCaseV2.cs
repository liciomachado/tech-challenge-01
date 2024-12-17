using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Validations;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.UseCases;

public class InsertContactUseCaseV2([FromServices] IContactPublisher contactPublisher /* Serviço dedicado para publicar */) : IInsertContactUseCaseV2
{
    public async Task<PublishResponse> Execute(InsertContactRequest insertContactRequest)
    {
        if (!ContactValidator.IsValidName(insertContactRequest.Name))
            throw new ArgumentException("O nome é obrigatório.");

        if (!ContactValidator.IsValidEmail(insertContactRequest.Email))
            throw new ArgumentException($"Formato de e-mail inválido.");

        var phoneNumber = new PhoneNumber(insertContactRequest.PhoneNumber);

        var contact = new Contact(insertContactRequest.Name, phoneNumber, insertContactRequest.Email);

        // Cria a mensagem e publica na fila
        await contactPublisher.PublishInsertContactAsync(new InsertContactEvent
        {
            Name = insertContactRequest.Name,
            Email = insertContactRequest.Email,
            PhoneNumber = insertContactRequest.PhoneNumber
        });

        return new PublishResponse
        {
            Message = "Cadastro em processamento.",
            Data = new
            {
                insertContactRequest.Name,
                insertContactRequest.Email,
                insertContactRequest.PhoneNumber
            }
        };
    }
}


