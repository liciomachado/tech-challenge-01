using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces;

public interface IGetContactsUseCase
{
    Task<List<ContactResponse>> Execute(string? ddd);
    Task<List<ContactResponse>> GetAll();

    Task<ContactResponse?> GetByIdAsync(long id);
}
