// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

[OptionName("Application")]
public class ApplicationOption
{
    public string AssemblyPath { get; set; } = null!;
    public string DataDirectory { get; set; } = null!;
    public string Version { get; set; } = null!;
}
