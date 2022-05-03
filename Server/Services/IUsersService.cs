public interface IUsersService
{
    Task<List<string>> GetFollowersAsync(string id);
    Task<List<UserDTO>> GetAsync();
    Task<UserDTO?> GetAsync(string id);
    Task<UserDTO?> GetUsernameAsync(string username);
    Task<string> GetSalt(string username);
    Task<UserDTO?> Signin(string username, string password);
    Task<Status> CreateAsync(User newUser);
    Task UpdateAsync(string id, User updatedUser);
    Task RemoveAsync(string id);
    Task<Status> Follow(string whoID, string whomID);
    Task<Status> Unfollow(string whoID, string whomID);
}
