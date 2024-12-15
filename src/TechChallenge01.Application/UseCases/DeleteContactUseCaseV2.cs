using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Events;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Application.UseCases;

public class DeleteContactUseCaseV2([FromServices] IContactPublisher contactPublisher /* Serviço dedicado para publicar */,
    IContactRepository contactRepository) : IDeleteContactsUseCaseV2
{
    public async Task<PublishResponse> Delete(long id)
    {
        var contact = await contactRepository.GetByIdAsync(id);

        if (contact is null)
            throw new ApplicationException("Contato não encontrado");

        await contactPublisher.PublishDeleteContactAsync(new DeleteContactEvent { Id = id });

        return new PublishResponse 
        { 
            Message = "Exclusão em processamento.",
            Data = new 
            { 
                contact.Id, 
                contact.Name, 
                contact.Email, 
                PhoneNumber = contact.PhoneNumber.DDD + " " + contact.PhoneNumber.Value 
            }
        };
    }
}


