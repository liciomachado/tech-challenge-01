using MassTransit;
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
    public class UpdateContactConsumer : IConsumer<UpdateContactEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public UpdateContactConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<UpdateContactEvent> context)
        {
            var message = context.Message;

            try
            {
                // Grava contato no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IContactRepository>();

                    var contact = await scopedProcessingService.GetByIdAsync(message.Id);

                    contact.Update(message.Name, new PhoneNumber(message.PhoneNumber), message.Email);
                    scopedProcessingService.Update(contact);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Contato atualizado com sucesso: {contact.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: Email:{message.Email} {ex.Message}");
            }
        }
    }
}
