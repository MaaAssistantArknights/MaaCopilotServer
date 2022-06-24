// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;

namespace MaaCopilotServer.Application.Common.Exceptions;

/// <summary>
///     The exception that is thrown by the handler of requests.
/// </summary>
[ExcludeFromCodeCoverage]
public class PipelineException : Exception
{
    /// <summary>
    ///     The constructor of <see cref="PipelineException" />.
    /// </summary>
    /// <param name="result">The action result.</param>
    public PipelineException(MaaApiResponse<GetCopilotUserDto> result)
    {
        Result = result;
    }

    /// <summary>
    ///     The action result.
    /// </summary>
    public MaaApiResponse<GetCopilotUserDto> Result { get; }
}
