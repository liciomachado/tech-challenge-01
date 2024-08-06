using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces
{
    public interface ITokenServiceUseCase
    {
        public string GetToken(UsuarioToken usuario);
    }
}
