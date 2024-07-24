using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        /// <summary>
        /// Inclusão de um Contato
        /// </summary>
        /// <param name="insertContactUseCase">Contato a ser incluído</param>
        /// <param name="insertContactRequest">Contato a ser incluído</param>
        /// <returns>Retorna o Contato Incluído</returns>
        /// <response code="200">Sucesso na inclusão do Contato</response>
        /// <response code="500">Não foi possível incluir o Contato</response>
        [HttpPost]
        public async Task<IActionResult> Add([FromServices] IInsertContactUseCase insertContactUseCase, InsertContactRequest insertContactRequest)
        {
            try
            {
                return Ok(await insertContactUseCase.Execute(insertContactRequest));

            }
            catch (ApplicationException e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromServices] IUpdateContactUseCase updateContactUseCase, UpdateContactRequest updateContactRequest)
        {
            try
            {
                return Ok(await updateContactUseCase.Execute(updateContactRequest));

            }
            catch (ApplicationException e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Retorna os Contatos Incluídos
        /// </summary>
        /// <param name="getContactsUseCase"></param>
        /// <param name="ddd">Informe a Região para Consulta (DDD)</param>
        /// <returns>Retorna a </returns>
        /// <response code="200">Sucesso na execução do retorno dos Contatos</response>
        /// <response code="500">Não foi possível retornar os Contatos</response>
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IGetContactsUseCase getContactsUseCase, [FromQuery]string? ddd)
        {
            return Ok(await getContactsUseCase.Execute(ddd));
        }
    }
}
