using PeopleManagement_BO;
using PeopleManagement_BO.Transactions;
using PeopleManagement_Utility;

namespace PeopleManagement_BI;

public class TransactionsRespository(ISqlDataAccess sqlDataAccess, IAccountsRepository accountsRepository) : ITransactionsRespository
{
    private readonly ISqlDataAccess _dataAccess = sqlDataAccess;
    private readonly IAccountsRepository _accountsRepository = accountsRepository;


    public async Task<ReturnResponse> AddTransactionAsync(TransactionRequest transactionRequest)
    {
        var response = new ReturnResponse();
        var account = await _accountsRepository.FindAccountByCodeAsync(transactionRequest.AccountCode);
        if (account == null || account.AccountStatus == AccountStatus.Closed.ToString())
        {
            response.Message = "Cannot add transaction to a closed or non-existent account.";
            response.IsSuccess = false;
            response.Status = "Failure ";
            return response;
        }
        try
        {
            DateTime transactionDate = DateTime.Now;
            var results = await _dataAccess.UpsertData<ReturnResponse, dynamic>("dbo.spTransaction_AddTransaction",
                new
                {
                    transactionRequest.AccountCode,
                    TransactionDate = transactionDate,
                    CapturedDate = transactionDate,
                    transactionRequest.Amount,
                    transactionRequest.Description,
                });

            if (results.IsSuccess)
            {
                response.Message = results.Message;
                response.IsSuccess = true;
                response.Status = "Success";
            }
            else
            {
                response.Message = results.Message;
                response.IsSuccess = false;
                response.Status = "Failure";
            }
            return response;
        }
        catch (Exception ex)
        {
            response.Message = $"Failed to add transaction: {ex.Message}";
            response.IsSuccess = false;
            response.Status = "Failure";
            return response;
        }
    }
}
