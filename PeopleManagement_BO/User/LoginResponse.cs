namespace PeopleManagement_BO;

public class LoginResponse
{
    public required string JwtToken { get; set; }
    public DateTime Expiration { get; set; }
    public required string RefreshToken { get; set; }
}
