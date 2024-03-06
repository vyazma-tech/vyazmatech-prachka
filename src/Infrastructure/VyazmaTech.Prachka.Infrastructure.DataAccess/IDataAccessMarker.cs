using System.Reflection;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess;

public interface IDataAccessMarker
{
    public static Assembly Assembly => typeof(IDataAccessMarker).Assembly;
}