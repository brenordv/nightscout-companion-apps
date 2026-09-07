using Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Utils;

namespace Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Tests.Utils;

public class ValidatorsTests : IDisposable
{
    private const string SecretEnvVar = "SillySecret";
    private readonly string _originalSecret = Environment.GetEnvironmentVariable(SecretEnvVar);

    public void Dispose()
    {
        Environment.SetEnvironmentVariable(SecretEnvVar, _originalSecret);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsKeyValid_NullOrWhitespaceKey_ReturnsFalse(string key)
    {
        // Arrange
        Environment.SetEnvironmentVariable(SecretEnvVar, "some-secret");

        // Act
        var result = Validators.IsKeyValid(key);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsKeyValid_SecretNotConfigured_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable(SecretEnvVar, null);

        // Act
        var result = Validators.IsKeyValid("any-key");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsKeyValid_KeyMatchesSecret_ReturnsTrue()
    {
        // Arrange
        Environment.SetEnvironmentVariable(SecretEnvVar, "correct-horse-battery");

        // Act
        var result = Validators.IsKeyValid("correct-horse-battery");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsKeyValid_KeyDoesNotMatchSecret_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable(SecretEnvVar, "correct-horse-battery");

        // Act
        var result = Validators.IsKeyValid("wrong-key");

        // Assert
        Assert.False(result);
    }
}
