using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Application.UseCases;
public class GetContactsUseCase(IContactRepository contactRepository) : IGetContactsUseCase
{
    public async Task<List<ContactResponse>> Execute(int? ddd)
    {
        List<Contact> result = [];
        if (ddd is not null)
            result = await contactRepository.GetByDDD(ddd.Value);
        else
            result = await contactRepository.GetAll();

        var mapped = result.Select(x => new ContactResponse(x.Id, x.Name, x.PhoneNumber, x.Email, x.DDD.Value)).ToList();
        return mapped;
    }
}
