using System.Reflection;

namespace VyazmaTech.Prachka.Application.Core;

public interface IApplicationMarker
{
    public static Assembly Assembly => typeof(IApplicationMarker).Assembly;
}