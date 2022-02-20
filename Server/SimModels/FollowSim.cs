using System.Text.Json.Serialization;
using MiniTwit.Shared;

namespace MiniTwit.Server;

public class FollowSim
{
    [JsonPropertyName("follow")]
    public string? follow { get; set; }

    [JsonPropertyName("unfollow")]
    public string? unfollow { get; set; }

}