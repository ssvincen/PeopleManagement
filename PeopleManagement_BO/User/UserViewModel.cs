namespace PeopleManagement_BO;

public class UserViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string MobileNumber { get; set; }
    public bool MobileNumberConfirmed { get; set; }
    public string EmailAddress { get; set; }
    public string PasswordHash { get; set; }
    public bool EmailConfirmed { get; set; }
    public string OTP { get; set; }
    public string EmailOTP { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool Active { get; set; }
    public DateTime PasswordLastUpdate { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
