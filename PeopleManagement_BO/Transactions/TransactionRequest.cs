namespace PeopleManagement_BO.Transactions
{
    public class TransactionRequest
    {
        public int AccountCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
