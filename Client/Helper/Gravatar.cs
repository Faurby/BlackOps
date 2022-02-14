namespace Minitwit.Client.Helper;
using MiniTwit.Shared;
using System.Net.Http.Json;
public class Gravatar {
    public static async Task<string> GenerateGravatarLink(HttpClient http, string UserID, int size)
    {
        var user = await http.GetFromJsonAsync<User>("api/Users/" + UserID!);

        if (user.Email != null) // TODO: Why is this check nescesary?
        {
            var utilityDTO = await http.GetFromJsonAsync<UtilityDTO>("api/Utility/md5/" + user!.Email.Trim());

            return "https://www.gravatar.com/avatar/" + utilityDTO.hash + "?d=identicon&s=" + size;
        }
        else
        {
            var mockEmail = user.UserName + "@hotmail.com";
            var utilityDTO = await http.GetFromJsonAsync<UtilityDTO>("api/Utility/md5/" + mockEmail.Trim());

            return "https://www.gravatar.com/avatar/" + utilityDTO.hash + "?d=identicon&s=" + size;
        }
    }
}