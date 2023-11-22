using System.Reflection;

namespace Infrastructure.DataAccess;

public interface IDataAccessMarker
{
    public static Assembly Assembly => typeof(IDataAccessMarker).Assembly;
}