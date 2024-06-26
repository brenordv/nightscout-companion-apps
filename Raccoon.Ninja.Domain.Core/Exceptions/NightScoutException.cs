using System.Diagnostics.CodeAnalysis;

namespace Raccoon.Ninja.Domain.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class NightScoutException : Exception
{
    public NightScoutException()
    {
    }

    public NightScoutException(string message) : base(message)
    {
    }

    public NightScoutException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}