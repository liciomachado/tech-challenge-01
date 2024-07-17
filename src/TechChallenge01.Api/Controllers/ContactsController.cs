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
        /// Inserir um Contato
        /// </summary>
        /// <param name="insertContactUseCase"></param>
        /// <param name="insertContactRequest"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Buscar Contatos
        /// </summary>
        /// <param name="getContactsUseCase"></param>
        /// <param name="ddd"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IGetContactsUseCase getContactsUseCase, [FromQuery] int? ddd)
        {
            return Ok(await getContactsUseCase.Execute(ddd));
        }
    }
}
