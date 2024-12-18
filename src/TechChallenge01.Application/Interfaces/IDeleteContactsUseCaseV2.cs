using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces;

public interface IDeleteContactsUseCaseV2
{
    Task<PublishResponse> Delete(long Id);
}