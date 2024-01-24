using System.Reflection;

namespace Application.Core;

public interface IApplicationMarker
{
    public static Assembly Assembly => typeof(IApplicationMarker).Assembly;
}