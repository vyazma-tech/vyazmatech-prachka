using System.Collections;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Identity.ClassData;

internal sealed class ModeratorChangeRoleClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.UserRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.UserRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.UserRoleName };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}