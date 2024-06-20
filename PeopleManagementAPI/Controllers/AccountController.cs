using Microsoft.AspNetCore.Mvc;
using PeopleManagement_BI;
using PeopleManagement_BO;

namespace PeopleManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(ILogger<AccountController> logger,
        IAccountsRepository accountsRepository) : ControllerBase
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly IAccountsRepository _accountsRepository = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));


        [HttpPost("AddAccount")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAccount(AccountRequest accountRequest)
        {
            var result = await _accountsRepository.AddAccountAsync(accountRequest);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return Conflict(result.Message);
        }


        [HttpPost("CloseAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CloseAccount(int accountCode)
        {
            var result = await _accountsRepository.CloseAccountAsync(accountCode);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }



    }
}
