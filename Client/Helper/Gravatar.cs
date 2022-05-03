namespace MiniTwit.Client.Helper;
using MiniTwit.Shared;
using System.Net.Http.Json;
public class Gravatar {
    public static async Task<string> GenerateGravatarLink(HttpClient http, string UserID, int size)
    {
        var user = await http.GetFromJsonAsync<UserDTO>("api/Users/" + UserID!);
        var utilityDTO = await http.GetFromJsonAsync<UtilityDTO>("api/Utility/md5/" + user!.Email.Trim());

        return "https://www.gravatar.com/avatar/" + utilityDTO!.hash + "?d=identicon&s=" + size;
    }

    public static string GetColorFromEmail(UserDTO user) {
        var random = new Random(user.Email.GetHashCode());
        return String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
    }
}