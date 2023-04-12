// Copyright 2023 Cloud Native Foundation.
// Licensed under the Apache 2.0 license.
// See LICENSE file in the project root for full license information.

using CloudNative.CloudEvents.UnitTests;
using CloudNative.CloudEvents.UnitTests.ConformanceTestData;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CloudNative.CloudEvents.Xml.UnitTests;

public class Conformance
{
    private static readonly IReadOnlyList<ConformanceTest> allTests =
        TestDataProvider.Xml.LoadTests(ConformanceTestFile.FromXml, file => file.Tests);

    private static ConformanceTest GetTestById(string id) => allTests.Single(test => test.Id == id);
    private static IEnumerable<object[]> SelectTestIds(ConformanceTestType type) =>
        allTests
            .Where(test => test.TestType == type)
            .Select(test => new object[] { test.Id });

    public static IEnumerable<object[]> ValidEventTestIds => SelectTestIds(ConformanceTestType.ValidSingleEvent);
    public static IEnumerable<object[]> InvalidEventTestIds => SelectTestIds(ConformanceTestType.InvalidSingleEvent);
    public static IEnumerable<object[]> ValidBatchTestIds => SelectTestIds(ConformanceTestType.ValidBatch);
    public static IEnumerable<object[]> InvalidBatchTestIds => SelectTestIds(ConformanceTestType.InvalidBatch);

    [Theory, MemberData(nameof(ValidEventTestIds))]
    public void ValidEvent(string testId)
    {
        var test = GetTestById(testId);
        var extensions = test.SampleExtensionAttributes ? SampleEvents.SampleExtensionAttributes : null;
        CloudEvent expected = SampleEvents.FromId(test.SampleId);
        CloudEvent actual = new XmlEventFormatter().ConvertFromXElement(test.Element, extensions);
        TestHelpers.AssertCloudEventsEqual(expected, actual, TestHelpers.InstantOnlyTimestampComparer);
    }

    [Theory, MemberData(nameof(InvalidEventTestIds))]
    public void InvalidEvent(string testId)
    {
        var test = GetTestById(testId);
        var extensions = test.SampleExtensionAttributes ? SampleEvents.SampleExtensionAttributes : null;
        var formatter = new XmlEventFormatter();
        Assert.Throws<ArgumentException>(() => formatter.ConvertFromXElement(test.Element, extensions));
    }

    [Theory, MemberData(nameof(ValidBatchTestIds))]
    public void ValidBatch(string testId)
    {
        var test = GetTestById(testId);
        var extensions = test.SampleExtensionAttributes ? SampleEvents.SampleExtensionAttributes : null;
        IReadOnlyList<CloudEvent> expected = SampleBatches.FromId(test.SampleId);
        IReadOnlyList<CloudEvent> actual = new XmlEventFormatter().DecodeBatch(test.Element, extensions);
        TestHelpers.AssertBatchesEqual(expected, actual, TestHelpers.InstantOnlyTimestampComparer);
    }

    [Theory, MemberData(nameof(InvalidBatchTestIds))]
    public void InvalidBatch(string testId)
    {
        var test = GetTestById(testId);
        var extensions = test.SampleExtensionAttributes ? SampleEvents.SampleExtensionAttributes : null;
        var formatter = new XmlEventFormatter();
        Assert.Throws<ArgumentException>(() => formatter.DecodeBatch(test.Element, extensions));
    }
}
