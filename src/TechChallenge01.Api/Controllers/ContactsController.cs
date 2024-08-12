using Microsoft.AspNetCore.Authorization;
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
        /// <returns>Retorna o Contato incluído</returns>
        /// <response code="200">Sucesso na inclusão do Contato</response>
        /// <response code="400">Não foi possível incluir o Contato</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromServices] IInsertContactUseCase insertContactUseCase, InsertContactRequest insertContactRequest)
        {
            try
            {
                return Ok(await insertContactUseCase.Execute(insertContactRequest));

            }
            catch (Exception e) when (e is ApplicationException || e is ArgumentException)
            {
                return BadRequest(new ErrorMessageResponse(e.Message));
            }
        }

        /// <summary>
        /// Alteração de um Contato
        /// </summary>
        /// <param name="updateContactUseCase">Contato a ser alterado</param>
        /// <param name="updateContactRequest">Contato a ser alterado</param>
        /// <returns>Retorna o Contato alterado</returns>
        /// <response code="200">Sucesso na alteração do Contato</response>
        /// <response code="400">Não foi possível alterar o Contato</response>
        /// <response code="401">Não autorizado</response>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromServices] IUpdateContactUseCase updateContactUseCase, UpdateContactRequest updateContactRequest)
        {
            try
            {
                return Ok(await updateContactUseCase.Execute(updateContactRequest));

            }
            catch (Exception e) when (e is ApplicationException || e is ArgumentException)
            {
                return BadRequest(new ErrorMessageResponse(e.Message));
            }
        }

        /// <summary>
        /// Retorna todos os Contatos incluídos
        /// </summary>
        /// <param name="getContactsUseCase"></param>
        /// <returns>Retorna a </returns>
        /// <response code="200">Sucesso na execução do retorno dos Contatos</response>
        /// <response code="400">Não foi possível retornar os Contatos</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> Get([FromServices] IGetContactsUseCase getContactsUseCase)
        {
            return Ok(await getContactsUseCase.GetAll());
        }

        /// <summary>
        /// Retorna os Contatos incluídos
        /// </summary>
        /// <param name="getContactsUseCase">Retorna os Contatos incluídos</param>
        /// <param name="ddd">Informe a Região para Consulta (DDD)</param>
        /// <returns>Retorna a lista de Contatos incluídos</returns>
        /// <response code="200">Sucesso na execução do retorno dos Contatos</response>
        /// <response code="400">Não foi possível retornar os Contatos</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet("GetByDDD")]
        [Authorize]
        public async Task<IActionResult> Get([FromServices] IGetContactsUseCase getContactsUseCase, [FromQuery] string? ddd)
        {
            return Ok(await getContactsUseCase.Execute(ddd));
        }

        /// <summary>
        /// Retorna o  Contato  pelo Id
        /// </summary>
        /// <param name="getContactsUseCase"></param>
        /// <param name="Id">Informe o id do Contato (Id)</param>
        /// <returns>Retorna a </returns>
        /// <response code="200">Sucesso na execução do retorno do  Contato </response>
        /// <response code="400">Não foi possível retornar o  Contato </response>
        /// <response code="401">Não autorizado</response>
        [HttpGet("GetById")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync([FromServices] IGetContactsUseCase getContactsUseCase, [FromQuery] long Id)
        {
            return Ok(await getContactsUseCase.GetByIdAsync(Id));
        }

        /// <summary>
        /// Exclusão de um Contato
        /// </summary>
        /// <param name="deleteContactsUseCase">Exclusão de um Contato</param>
        /// <param name="Id">Identificador do Contato</param>
        /// <returns></returns>
        /// <response code="200">Sucesso na exclusão do Contato</response>
        /// <response code="400">Não foi possível excluir o Contato</response>
        /// <response code="401">Não autorizado</response>
        [HttpDelete]
        [Authorize]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromServices] IDeleteContactsUseCase deleteContactsUseCase, [FromQuery] long Id)
        {
            try
            {
                await deleteContactsUseCase.Delete(Id);
                return Ok();

            }
            catch (ApplicationException e)
            {
                return BadRequest(new ErrorMessageResponse(e.Message));
            }
        }
    }
}
