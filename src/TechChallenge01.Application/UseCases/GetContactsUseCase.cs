using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Application.UseCases;
public class GetContactsUseCase(IContactRepository contactRepository) : IGetContactsUseCase
{
    public async Task<List<InsertContactResponse>> Execute()
    {
        List<Contact> result = await contactRepository.GetAll();
        var mapped = result.Select(x => new InsertContactResponse(x.Id, x.Name, x.PhoneNumber, x.Email)).ToList();
        return mapped;
    }
}
