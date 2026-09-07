using Raccoon.Ninja.Fn.GlucoseMonitor.ExtensionMethods;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Fn.GlucoseMonitor.Tests.ExtensionMethods;

public class ListExtensionsTests
{
    [Fact]
    public void ToGlucoseReadings_WhenCalled_ShouldReturnCorrectResult()
    {
        // Arrange
        const int quantityDocuments = 2;
        var documents = MongoDocumentGenerators.NightScoutMongoDocumentMockList(quantityDocuments);

        var previousReading = Generators.GlucoseReadingMockSingle();

        // Act
        var result = documents.ToGlucoseReadings(previousReading).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(quantityDocuments, result.Count);
        Assert.Equal(documents[0].Value, result[0].Value);
        Assert.Equal(documents[1].Value, result[1].Value);
    }

    [Fact]
    public void ToGlucoseReadings_WhenCalledWillIgnoreNulls_ShouldReturnCorrectResult()
    {
        // Arrange
        const int quantityDocuments = 2;
        var documents = MongoDocumentGenerators.NightScoutMongoDocumentMockList(quantityDocuments);
        documents.Add(null);
        var previousReading = Generators.GlucoseReadingMockSingle();

        // Act
        var result = documents.ToGlucoseReadings(previousReading).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(quantityDocuments, result.Count);
        Assert.Equal(documents[0].Value, result[0].Value);
        Assert.Equal(documents[1].Value, result[1].Value);
    }
}
