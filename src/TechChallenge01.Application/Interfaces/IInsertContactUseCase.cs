using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces;

public interface IInsertContactUseCase
{
    Task<InsertContactResponse> Execute(InsertContactRequest insertContactRequest);
}
