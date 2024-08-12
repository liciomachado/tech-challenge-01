using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Application.UseCases;
public class GetContactsUseCase(IContactRepository contactRepository) : IGetContactsUseCase
{
    public async Task<List<ContactResponse>> Execute(string? ddd)
    {
        List<Contact> result = [];
        if (ddd is null)
            throw new ApplicationException("ddd não informado!");

        result = await contactRepository.GetByDDD(ddd);


        var mapped = result.Select(x => new ContactResponse(x.Id, x.Name, x.PhoneNumber.Value, x.Email, x.PhoneNumber.DDD)).ToList();
        return mapped;
    }

    public async Task<List<ContactResponse>> GetAll()
    {
        List<Contact> result = [];
        result = await contactRepository.GetAll();
        var mapped = result.Select(x => new ContactResponse(x.Id, x.Name, x.PhoneNumber.Value, x.Email, x.PhoneNumber.DDD)).ToList();
        return mapped;
    }

    public async Task<ContactResponse?> GetByIdAsync(long id)
    {

        var contact = await contactRepository.GetByIdAsync(id);
        if (contact is null)
            throw new ApplicationException("Contato não encontrado!");
        return new ContactResponse(contact.Id, contact.Name, contact.PhoneNumber.Value, contact.Email, contact.PhoneNumber.DDD);
    }
}
