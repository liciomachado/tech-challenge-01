using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Entities;

namespace TechChallenge01.Application.Interfaces;

public interface IGetContactsUseCase
{
    Task<List<ContactResponse>> Execute(string? ddd);
    Task<List<ContactResponse>> GetAll();

    Task<Contact?> GetByIdAsync(long id);
}
