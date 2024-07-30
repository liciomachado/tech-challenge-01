using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
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

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IGetContactsUseCase getContactsUseCase, [FromQuery] int? ddd)
        {
            return Ok(await getContactsUseCase.Execute(ddd));
        }

        [HttpDelete]
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
                return BadRequest(new { e.Message });
            }
        }
    }
}
