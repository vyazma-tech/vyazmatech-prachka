using System.Reflection;

namespace VyazmaTech.Prachka.Presentation.Endpoints;

public interface IEndpointsMarker
{
    public static Assembly Assembly => typeof(IEndpointsMarker).Assembly;
}