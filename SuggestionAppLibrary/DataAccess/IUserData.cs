namespace SuggestionAppLibrary.DataAccess;

public interface IUserData
{
    Task<List<UserModel>> GetUsersAsync();
    Task<UserModel> GetUserAsync(string id);
    Task<UserModel> GetUserFromAuthentication(string objectId);
    Task CrateUser(UserModel user);
    Task UpdateUser(UserModel user);
}