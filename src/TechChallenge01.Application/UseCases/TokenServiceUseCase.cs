using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TechChallenge01.Application.UseCases
{
    public class TokenServiceUseCase : ITokenServiceUseCase
    {
    
        private readonly IConfiguration _configuration;

        public TokenServiceUseCase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(UsuarioToken usuario)
        {
            // Validar se o usuário Existe
            // if !valido
            //return string.Empty;

            // Gerar o token p/ o Usuario
            var tokenHandler = new JwtSecurityTokenHandler();

            // Recuperar a chave no appSettings
            var chaveCriptografia = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT"));

            var tokenPropriedades = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, usuario.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chaveCriptografia),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenPropriedades);

            return tokenHandler.WriteToken(token);
        }
    }
}
