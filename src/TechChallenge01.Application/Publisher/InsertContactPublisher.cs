using MassTransit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Producer;

public class InsertContactPublisher
{
    private readonly IBus _bus;

    public InsertContactPublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishContactAsync(InsertContactRequest request)
    {
        var contactEvent = new InsertContactEvent
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        await _bus.Publish(contactEvent);
    }
}