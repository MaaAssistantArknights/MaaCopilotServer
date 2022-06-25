// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Net;
using System.Net.Mail;
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

        services.AddScoped<IIdentityService, IdentityService>();

        services.AddSingleton<ICopilotIdService, CopilotIdService>();
        services.AddSingleton<ISecretService, SecretService>();

        var smtpClient = new SmtpClient
        {
            Host = emailOption.Smtp.Host,
            Port = emailOption.Smtp.Port,
            EnableSsl = emailOption.Smtp.UseSsl,
            Credentials =
                emailOption.Smtp.UseAuthentication
                    ? new NetworkCredential(emailOption.Smtp.Account, emailOption.Smtp.Password)
                    : null,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Timeout = emailOption.Smtp.TimeoutMs
        };

        services
            .AddFluentEmail(emailOption.Sender.Address, emailOption.Sender.Name)
            .AddSmtpSender(smtpClient)
            .AddLiquidRenderer();

        services.AddTransient<IMailService, MailService>();

        return services;
    }
}
