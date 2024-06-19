using System.Collections;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Identity.ClassData;

public sealed class AdminChangeRoleClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.UserRoleName };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}