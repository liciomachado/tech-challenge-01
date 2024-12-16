﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.UseCases;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.Consumers
{
    public class DeleteContactConsumer : IConsumer<DeleteContactEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public DeleteContactConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<DeleteContactEvent> context)
        {
            var message = context.Message;

            try
            {
                // Deleta contato no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IContactRepository>();

                    var contact = await scopedProcessingService.GetByIdAsync(message.Id);

                    scopedProcessingService.Delete(contact);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Contato deletado com sucesso: {contact.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: Id:{message.Id} {ex.Message}");
            }
        }
    }
}