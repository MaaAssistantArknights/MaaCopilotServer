// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Exceptions;

/// <summary>
/// The exception that is thrown by the handler of requests.
/// </summary>
public class PipelineException : Exception
{
    /// <summary>
    /// The constructor of <see cref="PipelineException"/>.
    /// </summary>
    /// <param name="result">The action result.</param>
    public PipelineException(MaaActionResult<EmptyObject> result)
    {
        Result = result;
    }

    /// <summary>
    /// The action result.
    /// </summary>
    public MaaActionResult<EmptyObject> Result { get; }
}
