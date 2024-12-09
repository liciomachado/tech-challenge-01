using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.UseCases;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Consumers
{
    public class UpdateContactConsumer : IConsumer<UpdateContactMenssage>
    {
        private readonly IUpdateContactUseCase _updateContactUseCase;

        public UpdateContactConsumer(IUpdateContactUseCase updateContactUseCase)
        {
            _updateContactUseCase = updateContactUseCase;
        }

        public async Task Consume(ConsumeContext<UpdateContactMenssage> context)
        {
            var message = context.Message;

            try
            {
                // Converte a mensagem em uma solicitação para o caso de uso
                var request = new UpdateContactRequest(message.Id,message.Name, message.PhoneNumber, message.Email);

                System.Threading.Thread.Sleep(10000);
                // Executa o caso de uso
                var result = await _updateContactUseCase.Execute(request);

                Console.WriteLine($"Contato atualizado com sucesso: {result.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizado mensagem:email{message.Email} {ex.Message}");
            }
        }
    }
 }
