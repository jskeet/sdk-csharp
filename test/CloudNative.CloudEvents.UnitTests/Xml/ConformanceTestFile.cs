// Copyright 2023 Cloud Native Foundation.
// Licensed under the Apache 2.0 license.
// See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CloudNative.CloudEvents.Xml.UnitTests;

#nullable disable

public class ConformanceTestFile
{
    public IReadOnlyList<ConformanceTest> Tests { get; }

    private ConformanceTestFile(IEnumerable<ConformanceTest> tests)
    {
        Tests = tests.ToList().AsReadOnly();
    }

    public static ConformanceTestFile FromXml(string xml)
    {
        var doc = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
        var root = doc.Root;
        
        ConformanceTestType? defaultTestType = (string) root.Attribute("testType") is string attr ? Enum.Parse<ConformanceTestType>(attr) : null;

        return new ConformanceTestFile(root.Elements("test").Select(ConvertTest));

        ConformanceTest ConvertTest(XElement element) =>
            new()
            {
                Id = (string) element.Attribute("id"),
                Description = (string) element.Attribute("description"),
                // Every conformance test has exactly one child element.
                Element = element.Elements().Single(),
                TestType = (string) element.Attribute("testType") is string attr ? Enum.Parse<ConformanceTestType>(attr) : defaultTestType,
                SampleId = (string) element.Attribute("sampleId"),
                SampleExtensionAttributes = (string) element.Attribute("sampleExtensionAttributes") is "true"
            };
    }

}

public class ConformanceTest
{
    public string Id { get; set; }
    public string Description { get; set; }
    public string SampleId { get; set; }
    public bool SampleExtensionAttributes { get; set; }
    public XElement Element { get; set; }
    public ConformanceTestType? TestType { get; set; }
}

public enum ConformanceTestType
{
    ValidSingleEvent,
    ValidBatch,
    InvalidSingleEvent,
    InvalidBatch
}