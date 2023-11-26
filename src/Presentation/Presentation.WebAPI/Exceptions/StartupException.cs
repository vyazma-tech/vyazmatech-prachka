namespace Presentation.WebAPI.Exceptions;

public class StartupException : Exception
{
    public StartupException() : base() {}

    public StartupException(string? message) : base(message) {}
}