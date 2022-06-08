// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

public record GetCopilotOperationQuery : IRequest<MaaActionResult<GetCopilotOperationQueryDto>>
{
    public string? Id { get; set; }
}

public class
    GetCopilotOperationQueryHandler : IRequestHandler<GetCopilotOperationQuery,
        MaaActionResult<GetCopilotOperationQueryDto>>
{
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;

    public GetCopilotOperationQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICopilotIdService copilotIdService,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _copilotIdService = copilotIdService;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaActionResult<GetCopilotOperationQueryDto>> Handle(GetCopilotOperationQuery request,
        CancellationToken cancellationToken)
    {
        var id = _copilotIdService.DecodeId(request.Id!);
        if (id is null)
        {
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id)));
        }

        var entity = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        if (entity is null)
        {
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id)));
        }

        var dto = new GetCopilotOperationQueryDto(
            request.Id!, entity.StageName, entity.MinimumRequired,
            entity.CreateAt.ToStringZhHans(), entity.Content, entity.Author.UserName, entity.Title, entity.Details, entity.Downloads);
        return MaaApiResponse.Ok(dto, _currentUserService.GetTrackingId());
    }
}
