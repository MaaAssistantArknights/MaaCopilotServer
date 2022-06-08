// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Enum;

public struct LoggingType
{
    private readonly string _value;

    private LoggingType(string value)
    {
        _value = value;
    }

    public static LoggingType Request => new("Request");
    public static LoggingType LongRunRequest => new("Long Run Request");
    public static LoggingType FailedRequest => new("Failed Request");
    public static LoggingType Exception => new("Exception");

    public static implicit operator string(LoggingType loggingType)
    {
        return loggingType._value;
    }
}
