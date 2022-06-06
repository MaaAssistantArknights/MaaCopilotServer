// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.ValueObjects;

/// <summary>
/// User permission value object
/// </summary>
public class UserPermission : ValueObject
{
    public string Permission { get; private set; } = "User";

    private UserPermission() { }
    private UserPermission(string permission)
    {
        Permission = permission;
    }

    public static UserPermission From(string permission)
    {
        var perm = new UserPermission(permission);

        if (AvailablePermissions.Contains(perm) is false)
        {
            throw new PermissionNotExistException(permission);
        }

        return perm;
    }

    public static UserPermission Doctor => new("Doctor");
    public static UserPermission PrtsMaintainer => new("PrtsMaintainer");
    public static UserPermission PrtsAdministrator => new("PrtsAdministrator");

    public static IEnumerable<UserPermission> AvailablePermissions
    {
        get
        {
            yield return Doctor;
            yield return PrtsMaintainer;
            yield return PrtsAdministrator;
        }
    }

    public override string ToString()
    {
        return Permission;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Permission;
    }
}
