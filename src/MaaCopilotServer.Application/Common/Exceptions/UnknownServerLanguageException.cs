// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Exceptions;

public class UnknownServerLanguageException : Exception
{
    public UnknownServerLanguageException() : base("Unknown server language.") { }
}
