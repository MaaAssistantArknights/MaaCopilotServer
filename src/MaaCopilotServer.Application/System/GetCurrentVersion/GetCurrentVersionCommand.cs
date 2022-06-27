// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Options;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.System.GetCurrentVersion;

public record GetCurrentVersionCommand : IRequest<MaaApiResponse>;

public class GetCurrentVersionCommandHandler : IRequestHandler<GetCurrentVersionCommand, MaaApiResponse>
{
    private readonly IOptions<ApplicationOption> _applicationOptions;

    public GetCurrentVersionCommandHandler(IOptions<ApplicationOption> applicationOptions)
    {
        _applicationOptions = applicationOptions;
    }

    public async Task<MaaApiResponse> Handle(GetCurrentVersionCommand request, CancellationToken cancellationToken)
    {
        var dto = new GetCurrentVersionDto(_applicationOptions.Value.Version, DateTimeOffset.UtcNow.ToIsoString());
        return await Task.FromResult(MaaApiResponseHelper.Ok(dto));
    }
}
