// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Exceptions;

public class PipelineException : Exception
{
    public PipelineException(MaaActionResult<EmptyObject> result)
    {
        Result = result;
    }

    public MaaActionResult<EmptyObject> Result { get; }
}
