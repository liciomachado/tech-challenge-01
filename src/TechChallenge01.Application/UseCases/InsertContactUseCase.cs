using System.Text.RegularExpressions;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.UseCases;

public class InsertContactUseCase(IContactRepository contactRepository) : IInsertContactUseCase
{
    public async Task<ContactResponse> Execute(InsertContactRequest insertContactRequest)
    {
        var phoneNumber = new PhoneNumber(insertContactRequest.PhoneNumber);

        var contact = new Contact(insertContactRequest.Nome, phoneNumber, insertContactRequest.Email);
        contactRepository.Save(contact);
        await contactRepository.UnitOfWork.Commit();

        return new ContactResponse(contact.Id, contact.Name, contact.PhoneNumber.Value, contact.Email, contact.PhoneNumber.DDD);
    }
}


