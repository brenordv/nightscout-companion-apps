using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;

namespace Raccoon.Ninja.AzFn.DataApi.Tests.ExtensionMethods;

public class HttpRequestExtensionsTests
{
    [Fact]
    public void TryGetReadSinceParam_NoParameter_ReturnsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;

        // Act
        var result = request.TryGetReadSinceParam();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetReadSinceParam_InvalidParameter_ReturnsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.QueryString = new QueryString("?readSince=notANumber");

        // Act
        var result = request.TryGetReadSinceParam();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetReadSinceParam_ValidParameter_ReturnsLongValue()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.QueryString = new QueryString("?readSince=123456");

        // Act
        var result = request.TryGetReadSinceParam();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(123456L);
    }

    [Fact]
    public void TryGetReadSinceParam_ValidParameterSetThroughQueryProperty_ReturnsLongValue()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;

        // Setting Query directly using a dictionary.
        // Although Query is read-only, we can reassign it using the constructor of QueryCollection.
        var queryDictionary = new Dictionary<string, StringValues>
        {
            { "readSince", "987654" }
        };
        request.QueryString = QueryString.Create(queryDictionary);

        // Act
        var result = request.TryGetReadSinceParam();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(987654L);
    }
}