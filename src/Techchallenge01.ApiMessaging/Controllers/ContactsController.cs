using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.ApiMessaging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController :   ControllerBase
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
        public async Task<IActionResult> Add([FromServices] IInsertContactUseCaseV2 insertContactUseCase, InsertContactRequest insertContactRequest)
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
        public async Task<IActionResult> Update([FromServices] IUpdateContactUseCaseV2 updateContactUseCase, UpdateContactRequest updateContactRequest)
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
        /// Exclusão de um Contato
        /// </summary>
        /// <param name="deleteContactsUseCase">Exclusão de um Contato</param>
        /// <param name="Id">Identificador do Contato</param>
        /// <returns></returns>
        /// <response code="200">Sucesso na exclusão do Contato</response>
        /// <response code="400">Não foi possível excluir o Contato</response>
        /// <response code="401">Não autorizado</response>
        [HttpDelete]     
        [Route("delete")]
        public async Task<IActionResult> Delete([FromServices] IDeleteContactsUseCaseV2 deleteContactsUseCase, [FromQuery] long Id)
        {
            try
            {
                return Ok(await deleteContactsUseCase.Delete(Id));
            }
            catch (ApplicationException e)
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
    }
}
