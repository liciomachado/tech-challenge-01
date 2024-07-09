using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Application.UseCases;

public class InsertContactUseCase(IContactRepository contactRepository) : IInsertContactUseCase
{
    public async Task<InsertContactResponse> Execute(InsertContactRequest insertContactRequest)
    {
        var contact = new Contact(insertContactRequest.Nome, insertContactRequest.PhoneNumber, insertContactRequest.Email);
        contactRepository.Save(contact);
        await contactRepository.UnitOfWork.Commit();

        return new InsertContactResponse(contact.Id, contact.Name, contact.PhoneNumber, contact.Email);
    }
}
