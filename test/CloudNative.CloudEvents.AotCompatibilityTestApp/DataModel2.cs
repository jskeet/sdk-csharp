using System.Text.Json.Serialization;

namespace CloudNative.CloudEvents.AotCompatibilityTestApp;

public class DataModel2
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }
}
