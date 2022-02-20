using System.Text.Json.Serialization;
using MiniTwit.Shared;

namespace MiniTwit.Server;

public class MessageSim
{
    [JsonPropertyName("content")]
    public string content { get; set; }

}