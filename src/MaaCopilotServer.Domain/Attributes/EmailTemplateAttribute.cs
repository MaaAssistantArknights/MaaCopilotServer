// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Attributes;

/// <summary>
///     Attribute used to mark a class as email model and bind to a specific email template.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EmailTemplateAttribute : Attribute
{
    /// <summary>
    ///     Mark as a email model.
    /// </summary>
    /// <param name="templateName">The email template name.</param>
    public EmailTemplateAttribute(string templateName)
    {
        TemplateName = templateName;
    }

    /// <summary>
    ///     Email template name.
    /// </summary>
    public string TemplateName { get; }
}
