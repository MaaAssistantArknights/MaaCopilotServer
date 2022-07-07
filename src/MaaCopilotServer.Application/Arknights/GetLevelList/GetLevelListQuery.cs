// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Arknights.GetLevelList;

/// <summary>
///     The DTO for the get level list query.
/// </summary>
public record GetLevelListQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The server language. Could be (ignore case) Chinese, English, Japanese, Korean.
    /// </summary>
    [FromQuery(Name = "server")]
    public string Server { get; set; } = string.Empty;
}

public class GetLevelListQueryHandler : IRequestHandler<GetLevelListQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;

    public GetLevelListQueryHandler(IMaaCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MaaApiResponse> Handle(GetLevelListQuery request, CancellationToken cancellationToken)
    {
        var server = string.IsNullOrEmpty(request.Server) ? "chinese" : request.Server.ToLower();

        var query = _dbContext.ArkLevelData.AsQueryable();

        switch (server)
        {
            case "chinese":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameCn) == false &&
                    string.IsNullOrEmpty(x.CatOneCn) == false &&
                    string.IsNullOrEmpty(x.CatTwoCn) == false &&
                    string.IsNullOrEmpty(x.CatThreeCn) == false);
                break;
            case "english":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameEn) == false &&
                    string.IsNullOrEmpty(x.CatOneEn) == false &&
                    string.IsNullOrEmpty(x.CatTwoEn) == false &&
                    string.IsNullOrEmpty(x.CatThreeEn) == false);
                break;
            case "japanese":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameJp) == false &&
                    string.IsNullOrEmpty(x.CatOneJp) == false &&
                    string.IsNullOrEmpty(x.CatTwoJp) == false &&
                    string.IsNullOrEmpty(x.CatThreeJp) == false);
                break;
            case "korean":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameKo) == false &&
                    string.IsNullOrEmpty(x.CatOneKo) == false &&
                    string.IsNullOrEmpty(x.CatTwoKo) == false &&
                    string.IsNullOrEmpty(x.CatThreeKo) == false);
                break;
        }

        var data = await query.ToListAsync(cancellationToken);
        var dto = data.Select(x => x.MapToDto(server)).ToList();
        return MaaApiResponseHelper.Ok(dto);
    }
}