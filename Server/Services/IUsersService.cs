public interface IUsersService
{
    Task<List<string>> GetFollowersAsync(string id);
    Task<List<UserDTO>> GetAsync();
    Task<User?> GetAsync(string id);
    Task<User?> GetUsernameAsync(string username);
    Task<string> GetSalt(string username);
    Task<User?> Signin(string username, string password);
    Task<Status> CreateAsync(User newUser);
    Task UpdateAsync(string id, User updatedUser);
    Task RemoveAsync(string id);
    Task<Status> Follow(string whoID, string whomID);
    Task<Status> Unfollow(string whoID, string whomID);
}
