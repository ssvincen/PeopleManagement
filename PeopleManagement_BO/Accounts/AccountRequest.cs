namespace PeopleManagement_BO;

public class AccountRequest
{
    public int PersonCode { get; set; }
    public string AccountNumber { get; set; }
    public decimal OutstandingBalance { get; set; }
}
