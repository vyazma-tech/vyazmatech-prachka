using System.Reflection;

namespace Application.Handlers;

public interface IApplicationHandlersMarker
{
    public static Assembly Assembly => typeof(IApplicationHandlersMarker).Assembly;
}