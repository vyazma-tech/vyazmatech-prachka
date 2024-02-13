using System.Reflection;

namespace VyazmaTech.Prachka.Application.Handlers;

public interface IApplicationHandlersMarker
{
    public static Assembly Assembly => typeof(IApplicationHandlersMarker).Assembly;
}