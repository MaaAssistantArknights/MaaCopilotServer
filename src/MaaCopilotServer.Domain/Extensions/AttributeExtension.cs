// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Extensions;

public static class AttributeExtension
{
    public static T[] ReadAttributes<T>(this Type t) where T : Attribute
    {
        var attr = (T[])t.GetCustomAttributes(typeof(T), false);
        return attr;
    }

    public static T? ReadAttribute<T>(this Type t) where T : Attribute
    {
        return t.ReadAttributes<T>().FirstOrDefault();
    }
}
