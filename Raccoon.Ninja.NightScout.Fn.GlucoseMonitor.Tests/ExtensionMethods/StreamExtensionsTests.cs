using System.Text;
using Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.ExtensionMethods;

namespace Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Tests.ExtensionMethods;

public class StreamExtensionsTests
{
    [Fact]
    public async Task ExtractKey_ValidJsonWithKey_ReturnsKey()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"key\":\"my-secret-key\"}"));

        // Act
        var result = await stream.ExtractKeyAsync();

        // Assert
        Assert.Equal("my-secret-key", result);
    }

    [Fact]
    public async Task ExtractKey_JsonWithoutKey_ReturnsEmptyString()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"other\":\"value\"}"));

        // Act
        var result = await stream.ExtractKeyAsync();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task ExtractKey_EmptyStream_ReturnsEmptyString()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        // Act
        var result = await stream.ExtractKeyAsync();

        // Assert
        Assert.Equal(string.Empty, result);
    }
}
