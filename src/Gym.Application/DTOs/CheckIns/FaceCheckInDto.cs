using System.Text.Json.Serialization;

namespace Gym.Application.DTOs.CheckIns;

public class FaceCheckInDto
{
    [JsonPropertyName("faceEncoding")]
    public string? FaceEncoding { get; set; }
}
