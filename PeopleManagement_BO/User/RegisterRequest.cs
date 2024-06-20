namespace PeopleManagement_BO;

public class RegisterRequest
{

    public required string FirstName { get; set; }
    public required string Surname { get; set; }
    public required string MobileNumber { get; set; }
    public required string EmailAddress { get; set; }
    public required string Password { get; set; }
}
