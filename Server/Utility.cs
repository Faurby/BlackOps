using System.Text;
using MiniTwit.Shared;

namespace MiniTwit.Server;

public class Utility
{
    public Task<UtilityDTO> MD5(string s)
    {
        var u = new UtilityDTO();
        using var provider = System.Security.Cryptography.MD5.Create();
        StringBuilder builder = new StringBuilder();

        foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
            builder.Append(b.ToString("x2").ToLower());

        u.hash = builder.ToString();
        return Task.FromResult(u);
    }
}
