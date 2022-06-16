// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EmailTemplateAttribute : Attribute
{
    public string TemplateName { get; }

    public EmailTemplateAttribute(string templateName)
    {
        TemplateName = templateName;
    }
}
