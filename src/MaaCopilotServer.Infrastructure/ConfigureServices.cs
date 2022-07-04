// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using FluentEmail.MailKitSmtp;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database;
using MaaCopilotServer.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaaCopilotServer.Infrastructure;

/// <summary>
///     The extension to add infrastructure services.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    /// <summary>
    ///     Adds infrastructure services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configurations.</param>
    /// <returns>The service collection with the services added.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var emailOption = configuration.GetOption<EmailOption>();

        services.AddDbContext<IMaaCopilotDbContext, MaaCopilotDbContext>();

        services.AddSingleton<ICopilotIdService, CopilotIdService>();
        services.AddSingleton<ISecretService, SecretService>();

        var smtpClientOptions = new SmtpClientOptions
        {
            Server = emailOption.Smtp.Host,
            Port = emailOption.Smtp.Port,
            User = emailOption.Smtp.Account,
            Password = emailOption.Smtp.Password,
            RequiresAuthentication = emailOption.Smtp.UseAuthentication,
            UseSsl = emailOption.Smtp.UseSsl,
            UsePickupDirectory = false
        };

        services
            .AddFluentEmail(emailOption.Sender.Address, emailOption.Sender.Name)
            .AddMailKitSender(smtpClientOptions)
            .AddLiquidRenderer();

        services.AddTransient<IMailService, MailService>();

        return services;
    }
}
