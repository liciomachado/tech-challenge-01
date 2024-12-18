using MassTransit;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;

namespace TechChallenge01.Application.UseCases;

public class ContactPublisher : IContactPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ContactPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishInsertContactAsync(InsertContactEvent message)
    {
        await _publishEndpoint.Publish(message);
    }

    public async Task PublishUpdateContactAsync(UpdateContactEvent message)
    {
        await _publishEndpoint.Publish(message);
    }

    public async Task PublishDeleteContactAsync(DeleteContactEvent message)
    {
        await _publishEndpoint.Publish(message);
    }
}
