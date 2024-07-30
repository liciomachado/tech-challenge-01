using TechChallenge01.Application.Interfaces;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Application.UseCases;

public class DeleteContactUseCase(IContactRepository contactRepository) : IDeleteContactsUseCase
{
    public async Task Delete(long id)
    {
        var contact = await contactRepository.GetByIdAsync(id);

        if (contact is null)
            throw new ApplicationException("Contato não encontrado");

        contactRepository.Delete(contact);
        await contactRepository.UnitOfWork.Commit();
    }
}


