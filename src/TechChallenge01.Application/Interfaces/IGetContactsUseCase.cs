using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces;

public interface IGetContactsUseCase
{
    Task<List<InsertContactResponse>> Execute();
}
