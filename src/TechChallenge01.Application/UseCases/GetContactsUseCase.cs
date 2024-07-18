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
        if (ddd is not null)
            result = await contactRepository.GetByDDD(ddd);
        else
            result = await contactRepository.GetAll();

        var mapped = result.Select(x => new ContactResponse(x.Id, x.Name, x.PhoneNumber.Value, x.Email, x.PhoneNumber.DDD)).ToList();
        return mapped;
    }
}
