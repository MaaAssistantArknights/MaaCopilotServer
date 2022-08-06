// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;

namespace MaaCopilotServer.Application.System.Echo
{
    /// <summary>
    /// The DTO for the echo command.
    /// </summary>
    public record EchoCommand : IRequest<MaaApiResponse>;

    /// <summary>
    /// The handler of the echo command.
    /// </summary>
    public class EchoCommandHandler : IRequestHandler<EchoCommand, MaaApiResponse>
    {
        /// <inheritdoc/>
        public Task<MaaApiResponse> Handle(EchoCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(MaaApiResponseHelper.Ok());
        }
    }
}
