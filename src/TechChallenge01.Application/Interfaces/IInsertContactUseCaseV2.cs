using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces;

public interface IInsertContactUseCaseV2
{
    Task<PublishResponse> Execute(InsertContactRequest insertContactRequest);
}
