using System.Text.RegularExpressions;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Application.UseCases;

public class InsertContactUseCase(IContactRepository contactRepository) : IInsertContactUseCase
{
    public async Task<ContactResponse> Execute(InsertContactRequest insertContactRequest)
    {
        var formatedPhone = Regex.Replace(insertContactRequest.PhoneNumber, "[^0-9a-zA-Z]+", "");
        if (formatedPhone.Length > 11)
            throw new ApplicationException("Numero de telefone informado incorretamente, Modelo esperado: (dd) 99999-9999");

        var contact = new Contact(insertContactRequest.Nome, formatedPhone, insertContactRequest.Email);
        contactRepository.Save(contact);
        await contactRepository.UnitOfWork.Commit();

        return new ContactResponse(contact.Id, contact.Name, contact.PhoneNumber, contact.Email, contact.DDD.Value);
    }
}


