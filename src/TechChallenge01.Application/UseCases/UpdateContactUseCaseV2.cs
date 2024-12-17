using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Domain.Validations;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.UseCases
{
    public class UpdateContactUseCaseV2([FromServices] IContactPublisher contactPublisher /* Serviço dedicado para publicar */,
        IContactRepository contactRepository) : IUpdateContactUseCaseV2
    {
        public async Task<PublishResponse> Execute(UpdateContactRequest updateContactRequest)
        {
            var contact = await contactRepository.GetByIdAsync(updateContactRequest.Id);
            if (contact == null) throw new ApplicationException("Não foi possível localizar o cadastro do contato informado.");

            if (!ContactValidator.IsValidName(updateContactRequest.Name))
                throw new ArgumentException("O nome é obrigatório.");

            if (!ContactValidator.IsValidEmail(updateContactRequest.Email))
                throw new ArgumentException("Formato de e-mail inválido.");

            // Criado para validar as regras antes da publicação na fila
            var phoneNumber = new PhoneNumber(updateContactRequest.PhoneNumber);

            await contactPublisher.PublishUpdateContactAsync(new UpdateContactEvent
            {
                Id = updateContactRequest.Id,
                Name = updateContactRequest.Name,
                Email = updateContactRequest.Email,
                PhoneNumber = updateContactRequest.PhoneNumber
            });

            return new PublishResponse
            {
                Message = "Atualização em processamento.",
                Data = new
                {
                    updateContactRequest.Name,
                    updateContactRequest.Email,
                    updateContactRequest.PhoneNumber
                }
            };
        }
    }
}
