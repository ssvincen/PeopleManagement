using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManagement_BI;
using PeopleManagement_BO.Transactions;

namespace PeopleManagementAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(ITransactionsRespository transactionsRespository) : ControllerBase
    {
        private readonly ITransactionsRespository _transactionsRespository = transactionsRespository
            ?? throw new ArgumentNullException(nameof(transactionsRespository));


        [AllowAnonymous]
        [HttpPost("AddTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTransaction(TransactionRequest transactionRequest)
        {
            var result = await _transactionsRespository.AddTransactionAsync(transactionRequest);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
