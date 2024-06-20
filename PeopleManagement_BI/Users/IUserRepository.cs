using PeopleManagement_BO;

namespace PeopleManagement_BI;

public interface IUserRepository
{
    Task<UserViewModel> FindUserByUsernameAsync(string username);
    Task UpdateUserRefreshTokenAsync(string username, string refreshToken, DateTime refreshTokenExpiryTime);
    Task<ReturnResponse> AddUserAsync(UserModel userModel);
}
