using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManagement_BI;
using PeopleManagement_BO;

namespace PeopleManagementAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController(IPersonRepository personRepository) : ControllerBase
    {
        private readonly IPersonRepository _personRepository = personRepository
            ?? throw new ArgumentNullException(nameof(personRepository));


        [HttpGet("GetPersonList")]
        public async Task<IActionResult> GetPersonList(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var people = await _personRepository.GetPersonListAsync(DataFilter.PageFilterModel(searchString, pageNumber, pageSize));
            return Ok(people);
        }


        [HttpGet("GetPersonByCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonById(int code)
        {
            var person = await _personRepository.GetPersonByCodeAsync(code);
            return Ok(person);
        }


        [HttpPost("AddPerson")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPerson(PersonRequest personRequest)
        {
            var result = await _personRepository.AddPersonAsync(personRequest);
            if (result.IsSuccess)
            {
                return Ok("Person successfully created");
            }
            return Conflict(result.Message);
        }


        [HttpPut("UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePerson(PersonResponse personResponse)
        {
            var result = await _personRepository.UpdatePersonAsync(personResponse);
            if (result.IsSuccess)
            {
                return Ok("Person successfully updated");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to update Person: {result.Message}");
        }

        [AllowAnonymous]
        [HttpDelete("DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePerson(int code)
        {
            var result = await _personRepository.DeletePersonAsync(code);
            if (result.IsSuccess)
            {
                return Ok("Person successfully deleted");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to delete Person: {result.Message}");
        }
    }
}
