// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using FluentEmail.Core;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Extensions;
using MaaCopilotServer.Domain.Options;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly IOptions<ApplicationOption> _applicationOption;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFluentEmail _fluentEmail;

    public MailService(IFluentEmail fluentEmail, IOptions<ApplicationOption> applicationOption,
        ICurrentUserService currentUserService)
    {
        _fluentEmail = fluentEmail;
        _applicationOption = applicationOption;
        _currentUserService = currentUserService;
    }

    public async Task<bool> SendEmailAsync<T>(T model, string targetAddress) where T : class, IEmailModel
    {
        var attr = typeof(T).ReadAttribute<EmailTemplateAttribute>();
        var templateName = attr!.TemplateName;

        var templateFile = GetTemplateFilePath(_currentUserService.GetCulture(),
            Path.Combine(_applicationOption.Value.DataDirectory, "templates", $"{templateName}"));

        var email = _fluentEmail
            .To(targetAddress)
            .UsingTemplateFromFile(templateFile, model)
            .Subject("Maa Copilot");

        var sendResponse = await email.SendAsync();
        return sendResponse.Successful;
    }

    private static string GetTemplateFilePath(CultureInfo cultureInfo, string baseName)
    {
        var name = cultureInfo.Name.ToLower();
        if (name == "zh-cn")
        {
            return baseName + ".liquid";
        }

        // Search Full Name
        var f1 = $"{baseName}-{name}.liquid";
        if (File.Exists(f1))
        {
            return f1;
        }

        // Search Language Name
        var f2 = $"{baseName}-{cultureInfo.TwoLetterISOLanguageName}.liquid";
        if (File.Exists(f2))
        {
            return f2;
        }

        // Return default
        return baseName + ".liquid";
    }
}
