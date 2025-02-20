﻿using MassTransit;
using MassTransit.Testing;
 
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Publisher
{
    public class UpdateContactPublisher 
    {
        private readonly IBus _bus;

        public UpdateContactPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishContactAsync(UpdateContactRequest request)
        {
            var updateEvent = new UpdateContactEvent
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _bus.Publish(updateEvent);
        }
    }
}
