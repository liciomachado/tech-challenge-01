using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces;

public interface IInsertContactUseCase
{
    Task<ContactResponse> Execute(InsertContactRequest insertContactRequest);
}
