using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.UseCases;

public class AuthenticationUseCase(IConfiguration _configuration) : IAuthenticationUseCase
{
    public string GetToken(UserRequest usuario)
    {
        // Validar se o usuário Existe
        if ((string.IsNullOrWhiteSpace(usuario.Username)) || (string.IsNullOrWhiteSpace(usuario.Password)))
        {
            return string.Empty;
        }

        if ((usuario.Username != "admin") || (usuario.Password != "admin@123"))
        {
            return string.Empty;
        }
        // Gerar o token p/ o Usuario
        var tokenHandler = new JwtSecurityTokenHandler();

        // Recuperar a chave no appSettings
        var chaveCriptografia = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT")!);

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
