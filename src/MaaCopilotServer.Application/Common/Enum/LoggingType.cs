// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;

namespace MaaCopilotServer.Application.Common.Enum;

/// <summary>
///     The type of log.
/// </summary>
[ExcludeFromCodeCoverage]
public readonly struct LoggingType
{
    /// <summary>
    ///     The value.
    /// </summary>
    private readonly string _value;

    /// <summary>
    ///     The constructor of <see cref="LoggingType" />.
    /// </summary>
    /// <param name="value">The value.</param>
    private LoggingType(string value)
    {
        _value = value;
    }

    /// <summary>
    ///     The request log.
    /// </summary>
    public static LoggingType Request => new("Request");

    /// <summary>
    ///     The failed request log.
    /// </summary>
    public static LoggingType FailedRequest => new("Failed Request");

    /// <summary>
    ///     The exception.
    /// </summary>
    public static LoggingType Exception => new("Exception");

    /// <summary>
    ///     Exceptions thrown before reaching the mediator
    /// </summary>
    public static LoggingType MiddlewareException => new("Middleware Exception");

    /// <summary>
    ///     Worker services running report.
    /// </summary>
    public static LoggingType WorkerServicesReport => new("Worker Services Report");

    /// <summary>
    ///     Exception thrown by worker services.
    /// </summary>
    public static LoggingType WorkerServicesException => new("Worker Services Exception");

    /// <inheritdoc/>
    public override string ToString()
    {
        return _value;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is LoggingType other)
        {
            return _value == other._value;
        }
        
        return false;
    }

    /// <summary>
    ///     Check if two <see cref="LoggingType" /> are equal.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(LoggingType other)
    {
        return _value == other._value;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(LoggingType? a, LoggingType? b) => a.Equals(b);
    public static bool operator !=(LoggingType? a, LoggingType? b) => !a.Equals(b);
}
