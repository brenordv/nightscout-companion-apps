using System;

namespace Raccoon.Ninja.AzFn.DataApi.Utils;

public static class Validators
{
    public static bool IsKeyValid(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return false;

        var secret = GetSecret();

        return !string.IsNullOrWhiteSpace(secret) && key.Equals(secret);
    }
    
    
    private static string GetSecret()
    {
        return Environment.GetEnvironmentVariable("SillySecret");
    }
}