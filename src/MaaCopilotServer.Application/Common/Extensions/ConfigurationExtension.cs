// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace MaaCopilotServer.Application.Common.Extensions;

/// <summary>
///     Extension methods for <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationExtension
{
    /// <summary>
    ///     Get an option instance.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    /// <typeparam name="T">The option class.</typeparam>
    /// <returns>An instance of type <typeparamref name="T" />.</returns>
    /// <exception cref="ArgumentNullException">The <typeparamref name="T" /> does not have <see cref="OptionNameAttribute" /> attribute.</exception>
    public static T GetOption<T>(this IConfiguration configuration) where T : class, new()
    {
        var option = new T();
        var attr = typeof(T).ReadAttribute<OptionNameAttribute>();
        if (attr is null)
        {
            throw new ArgumentNullException(typeof(T).FullName);
        }

        configuration.GetSection(attr.OptionName).Bind(option);
        return option;
    }
}
