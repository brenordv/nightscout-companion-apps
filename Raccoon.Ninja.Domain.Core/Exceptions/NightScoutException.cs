namespace Raccoon.Ninja.Domain.Core.Exceptions;

public class NightScoutException : Exception
{
    public NightScoutException(string message) : base(message)
    {
    }

    public NightScoutException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}