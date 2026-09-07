using System.Security.Cryptography;
using System.Text;

namespace Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Utils;

public static class Validators
{
    public static bool IsKeyValid(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return false;

        var secret = GetSecret();

        if (string.IsNullOrWhiteSpace(secret)) return false;

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(key),
            Encoding.UTF8.GetBytes(secret));
    }

    private static string GetSecret()
    {
        return Environment.GetEnvironmentVariable("SillySecret");
    }
}
