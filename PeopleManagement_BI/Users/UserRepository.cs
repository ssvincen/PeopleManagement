using PeopleManagement_BO;
using PeopleManagement_Utility;

namespace PeopleManagement_BI;

public class UserRepository(ISqlDataAccess sqlDataAccess) : IUserRepository
{
    private readonly ISqlDataAccess _dataAccess = sqlDataAccess;

    public async Task<ReturnResponse> AddUserAsync(UserModel userModel)
    {
        var results = await _dataAccess.UpsertData<ReturnResponse, dynamic>("dbo.spUsers_AddUser",
            new
            {
                userModel.FirstName,
                userModel.Surname,
                userModel.MobileNumber,
                userModel.EmailAddress,
                PasswordHash = userModel.Password,
                userModel.OTP,
                userModel.EmailOTP
            });
        return results;
    }

    public async Task<UserViewModel> FindUserByUsernameAsync(string username)
    {
        var results = await _dataAccess.LoadSingleData<UserViewModel, dynamic>(
             "dbo.spUsers_FindByUsername", new { EmailAddress = username });
        return results;
    }

    public async Task UpdateUserRefreshTokenAsync(string username, string refreshToken, DateTime refreshTokenExpiryTime)
    {
        await _dataAccess.UpdataData("spUsers_UpdateUserRefreshToken",
            new { EmailAddress = username, RefreshToken = refreshToken, RefreshTokenExpiryTime = refreshTokenExpiryTime });
    }
}
