using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Validations;

namespace TechChallenge01.Application.UseCases;

public class ContactPublisher : IContactPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ContactPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishContactAsync(InsertContactEvent message)
    {
        if (!ContactValidator.IsValidName(message.Name))
            throw new ArgumentException("O nome é obrigatório.");

        if (!ContactValidator.IsValidEmail(message.Email))
            throw new ArgumentException($"Formato de e-mail inválido.{message.Email}");


        await _publishEndpoint.Publish(message);
    }
}
