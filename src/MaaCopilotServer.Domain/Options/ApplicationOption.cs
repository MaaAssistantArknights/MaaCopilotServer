// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     Options about application itself.
/// </summary>
[OptionName("Application")]
[ExcludeFromCodeCoverage]
public class ApplicationOption
{
    /// <summary>
    ///     The directory where the application assembly is located.
    /// </summary>
    public string AssemblyPath { get; set; } = null!;
    /// <summary>
    ///     The email template directory.
    /// </summary>
    public string EmailTemplateDirectory { get; set; } = null!;
    /// <summary>
    ///     The version of the server.
    /// </summary>
    public string Version { get; set; } = null!;
}
