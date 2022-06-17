// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;

namespace MaaCopilotServer.Application.Common.Enum;

/// <summary>
/// The type of log.
/// </summary>
[ExcludeFromCodeCoverage]
public struct LoggingType
{
    /// <summary>
    /// The value.
    /// </summary>
    private readonly string _value;

    /// <summary>
    /// The constructor of <see cref="LoggingType"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    private LoggingType(string value)
    {
        _value = value;
    }

    /// <summary>
    /// The request log.
    /// </summary>
    public static LoggingType Request => new("Request");

    /// <summary>
    /// The failed request log.
    /// </summary>
    public static LoggingType FailedRequest => new("Failed Request");

    /// <summary>
    /// The exception.
    /// </summary>
    public static LoggingType Exception => new("Exception");

    /// <summary>
    /// The implicit operator from <see cref="LoggingType"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="loggingType">The logging type.</param>
    public static implicit operator string(LoggingType loggingType)
    {
        return loggingType._value;
    }
}
