using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces
{
    public interface IUpdateContactUseCaseV2
    {
        Task<PublishResponse> Execute(UpdateContactRequest updateContactRequest);
    }
}
