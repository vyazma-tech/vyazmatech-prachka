using System.Reflection;

namespace VyazmaTech.Prachka.Presentation.WebAPI;

public interface IWebAPIMarker
{
    public static Assembly Assembly => typeof(IWebAPIMarker).Assembly;
}