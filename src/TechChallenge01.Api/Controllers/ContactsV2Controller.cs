using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsV2Controller : ControllerBase
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
        public async Task<IActionResult> Add([FromServices] IInsertContactUseCaseV2 insertContactUseCase, InsertContactRequest insertContactRequest)
        {
            try
            {
                return Ok(await insertContactUseCase.Execute(insertContactRequest));
            }
            catch (Exception ex) when (ex is ApplicationException || ex is ArgumentException)
            {
                return BadRequest(new ErrorMessageResponse(ex.Message));
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
        public async Task<IActionResult> Update([FromServices] IUpdateContactUseCaseV2 updateContactUseCase, UpdateContactRequest updateContactRequest)
        {
            try
            {
                return Ok(await updateContactUseCase.Execute(updateContactRequest));

            }
            catch (Exception ex) when (ex is ApplicationException || ex is ArgumentException)
            {
                return BadRequest(new ErrorMessageResponse(ex.Message));
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
        [Authorize]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromServices] IDeleteContactsUseCaseV2 deleteContactsUseCase, [FromQuery] long Id)
        {
            try
            {
                return Ok(await deleteContactsUseCase.Delete(Id));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new ErrorMessageResponse(ex.Message));
            }
        }
    }
}
