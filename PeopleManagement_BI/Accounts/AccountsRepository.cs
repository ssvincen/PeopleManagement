using PeopleManagement_BO;
using PeopleManagement_Utility;

namespace PeopleManagement_BI;

public class AccountsRepository(ISqlDataAccess dataAccess) : IAccountsRepository
{
    private readonly ISqlDataAccess _dataAccess = dataAccess;

    public async Task<ReturnResponse> AddAccountAsync(AccountRequest accountRequest)
    {
        var results = await _dataAccess.UpsertData<ReturnResponse, dynamic>("dbo.spAccount_AddAccount",
           new { accountRequest.PersonCode, accountRequest.AccountNumber, OutstandingBalance = 0 });
        return results;
    }

    public async Task<ReturnResponse> CloseAccountAsync(int accountCode)
    {
        var results = await _dataAccess.UpsertData<ReturnResponse, dynamic>("dbo.spAccount_CloseAccount",
            new { AccountCode = accountCode });
        return results;
    }

    public async Task<AccountResponse> FindAccountByCodeAsync(int code)
    {
        var results = await _dataAccess.LoadSingleData<AccountResponse, dynamic>("dbo.spAccount_GetAccountByCode",
           new { Code = code });
        return results;
    }

    public Task<AccountResponse> FindAccountByAccountNumberAsync(string accountNumber)
    {
        throw new NotImplementedException();
    }

}
