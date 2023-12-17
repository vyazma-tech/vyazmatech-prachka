namespace Presentation.WebAPI.Exceptions;

public class StartupException : Exception
{
    public StartupException() { }

    public StartupException(string? message)
        : base(message) { }
}