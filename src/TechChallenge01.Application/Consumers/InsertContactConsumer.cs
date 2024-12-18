 
using MassTransit;
using System.Threading.Tasks;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
 
using TechChallenge01.Application.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.Consumers;

public class InsertContactConsumer : IConsumer<InsertContactEvent>
{
    private readonly IServiceProvider _serviceProvider;
    public InsertContactConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<InsertContactEvent> context)
    {
        var message = context.Message;

        try
        {
            var contact = new Contact(message.Name, new PhoneNumber(message.PhoneNumber), message.Email);

            // Grava contato no DB
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                    .GetRequiredService<IContactRepository>();


                scopedProcessingService.Save(contact);
                await scopedProcessingService.UnitOfWork.Commit();
            }

            // TODO: Remover
            System.Threading.Thread.Sleep(10000);

            Console.WriteLine($"Contato inserido com sucesso: {contact.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar mensagem:Email:{message.Email} {ex.Message}");
        }
    }
}
