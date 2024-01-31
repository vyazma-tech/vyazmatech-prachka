using System.Collections;
using Application.DataAccess.Contracts.Abstractions;

namespace Test.Handlers.Identity.ClassData;

public sealed class ChangeRoleClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.AdminRoleName };
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.ModeratorRoleName };
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.EmployeeRoleName };
        yield return new object[] { VyazmaTechRoleNames.AdminRoleName, VyazmaTechRoleNames.UserRoleName };
        yield return new object[] { VyazmaTechRoleNames.EmployeeRoleName, VyazmaTechRoleNames.UserRoleName };
        yield return new object[] { VyazmaTechRoleNames.ModeratorRoleName, VyazmaTechRoleNames.UserRoleName };
        yield return new object[] { VyazmaTechRoleNames.UserRoleName, VyazmaTechRoleNames.UserRoleName };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}