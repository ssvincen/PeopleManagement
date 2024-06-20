using PeopleManagement_BO;

namespace PeopleManagement_BI;

public interface IAccountsRepository
{
    Task<ReturnResponse> AddAccountAsync(AccountRequest accountRequest);
    Task<AccountResponse> FindAccountByAccountNumberAsync(string accountNumber);
    Task<AccountResponse> FindAccountByCodeAsync(int code);
    Task<ReturnResponse> CloseAccountAsync(int accountCode);

}
