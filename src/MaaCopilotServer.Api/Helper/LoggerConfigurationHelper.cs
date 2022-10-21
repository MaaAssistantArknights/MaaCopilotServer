// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using Destructurama;
using Serilog;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
///     The helper class of logger.
/// </summary>
[ExcludeFromCodeCoverage]
public static class LoggerConfigurationHelper
{
    /// <summary>
    ///     Constructs a <see cref="LoggerConfiguration" /> instance based on the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The <see cref="LoggerConfiguration" /> instance.</returns>
    public static LoggerConfiguration GetLoggerConfiguration(this IConfiguration configuration)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Destructure.UsingAttributes();

        return loggerConfiguration;
    }
}
