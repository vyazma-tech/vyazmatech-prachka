using System.Reflection;

namespace Presentation.WebAPI;

public interface IWebAPIMarker
{
    public static Assembly Assembly => typeof(IWebAPIMarker).Assembly;
}