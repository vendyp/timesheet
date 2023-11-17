using Timesheet.Shared.Abstractions.Models;

namespace Timesheet.Domain.Extensions;

public static class RoleExtensions
{
    public static Guid SuperAdministratorId = Guid.Empty;
    public static readonly string SuperAdministratorName = "Super Administrator";

    public static readonly List<DropdownKeyValue> AvailableRoles = new()
    {
        new DropdownKeyValue(SuperAdministratorId.ToString(), SuperAdministratorName)
    };

    public static string Slug(Guid id, string name)
    {
        //max length of database is 256, length of id is 64
        if (name.Length > 192)
            name = name[..192];

        var names = name.Split(' ');

        return $"{id:N}-{string.Join('-', names)}";
    }
}