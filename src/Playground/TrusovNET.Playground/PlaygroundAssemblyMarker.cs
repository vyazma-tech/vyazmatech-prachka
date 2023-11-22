using System.Reflection;

namespace TrusovNET.Playground;

public interface IPlaygroundMarker
{
    public static Assembly Assembly => typeof(IPlaygroundMarker).Assembly;
}