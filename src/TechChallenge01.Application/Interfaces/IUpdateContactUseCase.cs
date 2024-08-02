using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces
{
    public interface IUpdateContactUseCase
    {
        Task<ContactResponse> Execute(UpdateContactRequest updateContactRequest);
    }
}
