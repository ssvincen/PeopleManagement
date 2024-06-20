using PeopleManagement_BO;
using PeopleManagement_BO.Transactions;

namespace PeopleManagement_BI;

public interface ITransactionsRespository
{
    Task<ReturnResponse> AddTransactionAsync(TransactionRequest transactionRequest);
}
