namespace PeopleManagement_BO;

public class UserModel : RegisterRequest
{
    public string OTP { get; set; }
    public string EmailOTP { get; set; }
}
