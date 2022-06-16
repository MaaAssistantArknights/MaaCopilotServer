// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Extensions;

/// <summary>
/// The extension of attributes.
/// </summary>
public static class AttributeExtension
{
    /// <summary>
    /// Reads multiple attributes.
    /// </summary>
    /// <typeparam name="T">The <see cref="Attribute"/> type.</typeparam>
    /// <param name="t">The type.</param>
    /// <returns>The attributes.</returns>
    public static T[] ReadAttributes<T>(this Type t) where T : Attribute
    {
        var attr = (T[])t.GetCustomAttributes(typeof(T), false);
        return attr;
    }

    /// <summary>
    /// Reads an attribute.
    /// </summary>
    /// <typeparam name="T">The <see cref="Attribute"/> type.</typeparam>
    /// <param name="t">The type.</param>
    /// <returns>The attribute.</returns>
    public static T? ReadAttribute<T>(this Type t) where T : Attribute
    {
        return t.ReadAttributes<T>().FirstOrDefault();
    }
}
