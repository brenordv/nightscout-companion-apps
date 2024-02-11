using FluentAssertions;
using Raccoon.Ninja.AzFn.ScheduledTasks.ExtensionMethods;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.AzFn.ScheduledTasks.Tests.ExtensionMethods;

public class ListExtensionsTests
{
    [Fact]
    public void ToGlucoseReadings_WhenCalled_ShouldReturnCorrectResult()
    {
        // Arrange
        const int quantityDocuments = 2;
        var documents = Generators.NightScoutMongoDocumentMockList(quantityDocuments);

        var previousReading = Generators.GlucoseReadingMockSingle();

        // Act
        var result = documents.ToGlucoseReadings(previousReading).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(quantityDocuments);
        result[0].Value.Should().Be(documents[0].Value);
        result[1].Value.Should().Be(documents[1].Value);
    }
    
    [Fact]
    public void ToGlucoseReadings_WhenCalledWillIgnoreNulls_ShouldReturnCorrectResult()
    {
        // Arrange
        const int quantityDocuments = 2;
        var documents = Generators.NightScoutMongoDocumentMockList(quantityDocuments);
        documents.Add(null);
        var previousReading = Generators.GlucoseReadingMockSingle();

        // Act
        var result = documents.ToGlucoseReadings(previousReading).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(quantityDocuments);
        result[0].Value.Should().Be(documents[0].Value);
        result[1].Value.Should().Be(documents[1].Value);
    }
}
