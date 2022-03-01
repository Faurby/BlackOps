using System.Text.Json.Serialization;

namespace MiniTwit.Shared.SimModels;

public class LatestDTO
{
    [JsonPropertyName("latest")]
    public int latest { get; set; }
}