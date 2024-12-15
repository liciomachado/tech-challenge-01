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

    public async Task PublishInsertContacttAsync(InsertContactEvent message)
    {
        await _publishEndpoint.Publish(message);
    }

    public async Task PublishUpdateContacttAsync(UpdateContactEvent message)
    {
        await _publishEndpoint.Publish(message);
    }

    public async Task PublishDeleteContactAsync(DeleteContactEvent message)
    {
        await _publishEndpoint.Publish(message);
    }
}
