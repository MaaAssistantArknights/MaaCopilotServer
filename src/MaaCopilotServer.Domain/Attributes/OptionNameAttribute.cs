// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Attributes;

/// <summary>
///     The base attribute of options.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class OptionNameAttribute : Attribute
{
    /// <summary>
    ///     The constructor of <see cref="OptionNameAttribute" />.
    /// </summary>
    /// <param name="optionName">The option name.</param>
    public OptionNameAttribute(string optionName)
    {
        OptionName = optionName;
    }

    /// <summary>
    ///     The option name.
    /// </summary>
    public string OptionName { get; }
}
