using System.Text.Json.Serialization;

namespace CloudNative.CloudEvents.AotCompatibilityTestApp;

public class DataModel1
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }
}

[JsonSerializable(typeof(DataModel1))]
public partial class DataModel1Context : JsonSerializerContext { }
