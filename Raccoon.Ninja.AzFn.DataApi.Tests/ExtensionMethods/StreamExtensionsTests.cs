using System.IO;
using System.Text;
using System.Threading.Tasks;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;

namespace Raccoon.Ninja.AzFn.DataApi.Tests.ExtensionMethods;

public class StreamExtensionsTests
{
    [Fact]
    public async Task ExtractKey_ValidJsonWithKey_ReturnsKey()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"key\":\"my-secret-key\"}"));

        // Act
        var result = await stream.ExtractKey();

        // Assert
        Assert.Equal("my-secret-key", result);
    }

    [Fact]
    public async Task ExtractKey_JsonWithoutKey_ReturnsEmptyString()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"other\":\"value\"}"));

        // Act
        var result = await stream.ExtractKey();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task ExtractKey_EmptyStream_ReturnsEmptyString()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        // Act
        var result = await stream.ExtractKey();

        // Assert
        Assert.Equal(string.Empty, result);
    }
}
