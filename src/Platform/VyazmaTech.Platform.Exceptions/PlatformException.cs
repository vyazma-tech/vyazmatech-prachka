namespace VyazmaTech.Platform.Exceptions;

public sealed class PlatformException : Exception
{
    public PlatformException(string message) : base(message) { }
}