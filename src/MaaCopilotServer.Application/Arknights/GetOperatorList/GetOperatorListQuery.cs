// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Arknights.GetOperatorList;

/// <summary>
///     The DTO for the get operation list query.
/// </summary>
public record GetOperatorListQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The server language. Could be (ignore case) Chinese (cn), English (en), Japanese (ja), Korean (ko).
    /// </summary>
    [FromQuery(Name = "server")]
    public string Server { get; set; } = string.Empty;
}


public class GetOperatorListQueryHandler : IRequestHandler<GetOperatorListQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;

    private readonly Func<ArkCharacterInfo, GetOperatorListDto> _mapCn = data =>
        new GetOperatorListDto
        {
            Name = data.NameCn,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionCnString(),
            Star = data.Star
        };

    private readonly Func<ArkCharacterInfo, GetOperatorListDto> _mapEn = data =>
        new GetOperatorListDto
        {
            Name = data.NameEn,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionEnString(),
            Star = data.Star
        };

    private readonly Func<ArkCharacterInfo, GetOperatorListDto> _mapJp = data =>
        new GetOperatorListDto
        {
            Name = data.NameJp,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionJpString(),
            Star = data.Star
        };

    private readonly Func<ArkCharacterInfo, GetOperatorListDto> _mapKo = data =>
        new GetOperatorListDto
        {
            Name = data.NameKo,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionKoString(),
            Star = data.Star
        };

    public GetOperatorListQueryHandler(IMaaCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MaaApiResponse> Handle(GetOperatorListQuery request, CancellationToken cancellationToken)
    {
        var server = string.IsNullOrEmpty(request.Server) ? "chinese" : request.Server.ToLower();

        var query = _dbContext.ArkCharacterInfos.AsQueryable();

        var mapper = _mapCn;

        switch (server)
        {
            case "chinese" or "cn":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameCn) == false);
                mapper = _mapCn;
                break;
            case "english" or "en":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameEn) == false);
                mapper = _mapEn;
                break;
            case "japanese" or "ja":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameJp) == false);
                mapper = _mapJp;
                break;
            case "korean" or "ko":
                query = query.Where(x =>
                    string.IsNullOrEmpty(x.NameKo) == false);
                mapper = _mapKo;
                break;
        }

        var data = await query.ToListAsync(cancellationToken);
        var dto = data.Select(mapper).ToList();
        return MaaApiResponseHelper.Ok(dto);
    }
}
