 
using MassTransit;
using System.Threading.Tasks;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
 
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Consumers;

public class InsertContactConsumer : IConsumer<InsertContactEvent>
{
    private readonly IInsertContactUseCase _insertContactUseCase;

    public InsertContactConsumer(IInsertContactUseCase insertContactUseCase)
    {
        _insertContactUseCase = insertContactUseCase;
    }

    public async Task Consume(ConsumeContext<InsertContactEvent> context)
    {
        var message = context.Message;

        try
        {
            // Converte a mensagem em uma solicitação para o caso de uso
            var request = new InsertContactRequest(message.Name, message.PhoneNumber, message.Email);

            System.Threading.Thread.Sleep(10000);
            // Executa o caso de uso
            var result = await _insertContactUseCase.Execute(request);

            Console.WriteLine($"Contato inserido com sucesso: {result.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar mensagem:email{message.Email} {ex.Message}");
        }
    }
}
