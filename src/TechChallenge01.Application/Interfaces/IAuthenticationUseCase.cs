using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces
{
    public interface IAuthenticationUseCase
    {
        public string GetToken(UserRequest usuario);
    }
}
