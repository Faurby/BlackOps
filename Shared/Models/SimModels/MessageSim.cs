using System.Text.Json.Serialization;
using MiniTwit.Shared;

namespace MiniTwit.Shared.SimModels;

public class MessageSim
{
    [JsonPropertyName("content")]
    public string content { get; set; }

}