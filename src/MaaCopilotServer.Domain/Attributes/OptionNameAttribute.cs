// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OptionNameAttribute : Attribute
{
    public string OptionName { get; }

    public OptionNameAttribute(string optionName)
    {
        OptionName = optionName;
    }
}
