using Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Mongo;

namespace Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Tests.Mongo;

public class MongoCollectionBuilderTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddConnectionString_NullOrWhitespace_Throws(string value)
    {
        // Arrange
        var builder = new MongoCollectionBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.AddConnectionString(value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddDatabaseName_NullOrWhitespace_Throws(string value)
    {
        // Arrange
        var builder = new MongoCollectionBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.AddDatabaseName(value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddCollectionName_NullOrWhitespace_Throws(string value)
    {
        // Arrange
        var builder = new MongoCollectionBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.AddCollectionName(value));
    }

    [Fact]
    public void AddMethods_ValidValues_ReturnSameBuilderForChaining()
    {
        // Arrange
        var builder = new MongoCollectionBuilder();

        // Act
        var result = builder
            .AddConnectionString("mongodb://localhost:27017")
            .AddDatabaseName("nightscout")
            .AddCollectionName("entries");

        // Assert
        Assert.Same(builder, result);
    }
}
