// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.System.SendEmailTest;

/// <summary>
///     The DTO for the SendEmailTest command.
/// </summary>
public record SendEmailTestCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The target address
    /// </summary>
    [FromQuery(Name = "to")] public string? TargetAddress { get; set; }
    /// <summary>
    ///     The token.
    /// </summary>
    [FromQuery(Name = "token")] public string? Token { get; set; }
}

public class SendEmailTestCommandHandler : IRequestHandler<SendEmailTestCommand, MaaApiResponse>
{
    private readonly IMailService _mailService;
    private readonly IOptions<CopilotServerOption> _options;

    public SendEmailTestCommandHandler(IMailService mailService, IOptions<CopilotServerOption> options)
    {
        _mailService = mailService;
        _options = options;
    }

    public async Task<MaaApiResponse> Handle(SendEmailTestCommand request, CancellationToken cancellationToken)
    {
        // Validate the token.
        var validToken = request.Token == _options.Value.TestEmailApiToken;
        if (validToken is false)
        {
            return MaaApiResponseHelper.Unauthorized();
        }

        // Send email
        var to = request.TargetAddress.IsNotNull();
        var sendStatus = await _mailService
            .SendEmailAsync(new EmailSendTest(DateTimeOffset.UtcNow.ToUtc8String()), to);

        return sendStatus is false
            ? MaaApiResponseHelper.InternalError()
            : MaaApiResponseHelper.Ok();
    }
}
