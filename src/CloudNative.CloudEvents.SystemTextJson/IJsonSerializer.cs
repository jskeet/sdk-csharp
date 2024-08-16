using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudNative.CloudEvents.SystemTextJson;

/// <summary>
/// An abstraction of the static <see cref="JsonSerializer"/> methods, allowing appropriate
/// handling of AOT compliance.
/// </summary>
internal interface IJsonSerializer
{
    void SerializeToWriter(Utf8JsonWriter writer, object data);
    string SerializeToString(object data);
    ReadOnlyMemory<byte> SerializeToUtf8Bytes(object data);
    public object? Deserialize(JsonElement element, Type returnType);
    public object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType);
}

internal class JsonSerializerWithContext : IJsonSerializer
{
    private readonly JsonSerializerContext context;

    internal JsonSerializerWithContext(JsonSerializerContext context) =>
        this.context = context;

    public void SerializeToWriter(Utf8JsonWriter writer, object data) =>
        JsonSerializer.Serialize(writer, data, data.GetType(), context);

    public string SerializeToString(object data) =>
        JsonSerializer.Serialize(data, data!.GetType(), context);

    public ReadOnlyMemory<byte> SerializeToUtf8Bytes(object data) =>
        JsonSerializer.SerializeToUtf8Bytes(data, data.GetType(), context);

    public object? Deserialize(JsonElement element, Type returnType) =>
        JsonSerializer.Deserialize(element, returnType, context);

    public object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType) =>
        JsonSerializer.Deserialize(utf8Json, returnType, context);
}

internal class JsonSerializerWithoutContext : IJsonSerializer
{
    private readonly JsonSerializerOptions? options;

    internal JsonSerializerWithoutContext(JsonSerializerOptions? options) =>
        this.options = options;

#if NET5_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "Constructor already annotated.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "Constructor already annotated.")]
#endif
    public void SerializeToWriter(Utf8JsonWriter writer, object data) =>
        JsonSerializer.Serialize(writer, data, options);

#if NET5_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "Constructor already annotated.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "Constructor already annotated.")]
#endif
    public string SerializeToString(object data) =>
        JsonSerializer.Serialize(data, options);

#if NET5_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "Constructor already annotated.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "Constructor already annotated.")]
#endif
    public ReadOnlyMemory<byte> SerializeToUtf8Bytes(object data) =>
        JsonSerializer.SerializeToUtf8Bytes(data, options);

#if NET5_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "Constructor already annotated.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "Constructor already annotated.")]
#endif
    public object? Deserialize(JsonElement element, Type returnType) =>
        JsonSerializer.Deserialize(element, returnType, options);

#if NET5_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "Constructor already annotated.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "Constructor already annotated.")]
#endif
    public object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType) =>
        JsonSerializer.Deserialize(utf8Json, returnType, options);
}
