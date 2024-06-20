using PeopleManagement_BO;
using System.Text;

namespace PeopleManagement_Utility;

public static class Crypto
{
    public static string PasswordHash(string value)
    {
        return Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    }

    public static ReturnResponse ComparePassword(string password, string confirmPassword)
    {
        var response = new ReturnResponse
        {
            IsSuccess = false,
            Message = "Password don't match"
        };
        if (string.Compare(password, confirmPassword, true) == 0)
        {
            response.IsSuccess = true;
            response.Message = "Succeess Password Match";
        }
        return response;
    }
}
