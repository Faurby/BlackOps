using System.Text.Json.Serialization;
using MiniTwit.Shared;

namespace MiniTwit.Server;

public class RegisterSim
{
    [JsonPropertyName("username")]
    public string username { get; set; }

    [JsonPropertyName("email")]
    public string email { get; set; }

    [JsonPropertyName("pwd")]
    public string password { get; set; }

    public User ConvertToUser()
    {
        return new User
        {
            Email = email,
            UserName = username,
            Password = password
        };
    }
}