using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenServiceUseCase _tokenService;

        public TokenController(ITokenServiceUseCase tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Solicitação de token JwtBearer 
        /// </summary>
        /// <param name="usuario">Usuário do sistema</param>
        /// <returns>token</returns>
        /// <response code="200">Token fornecido com sucesso</response>
        /// <response code="401">Usuário/Senha inválido (não autorizado)</response>
        [HttpPost]
        public IActionResult Post([FromBody] UsuarioToken usuario)
        {
            var token = _tokenService.GetToken(usuario);

            if (!string.IsNullOrWhiteSpace(token))
            {
                return Ok(token);
            }

            return Unauthorized();
        }

    }
}
