// See https://aka.ms/new-console-template for more information
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.AotCompatibilityTestApp;
using CloudNative.CloudEvents.SystemTextJson;
using System.Text;

var evt = new CloudEvent
{
    Id = "test-event",
    Type = "test-type",
    Data = new DataModel2 { X = 5, Y = 10 },
    Source = new Uri("https://cloudevents.io")
};

var formatter = new JsonEventFormatter<DataModel2>(new DataModel1Context());
var bytes = formatter.EncodeStructuredModeMessage(evt, out _);

Console.WriteLine(Encoding.UTF8.GetString(bytes.Span));

var parsed = formatter.DecodeStructuredModeMessage(bytes, null, null);

var model = (DataModel2) parsed.Data!;
Console.WriteLine(model.X);
Console.WriteLine(model.Y);
