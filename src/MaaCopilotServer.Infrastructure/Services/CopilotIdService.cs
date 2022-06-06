// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using HashidsNet;
using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Infrastructure.Services;

public class CopilotIdService : ICopilotIdService
{
    private IHashids _hashids;

    public CopilotIdService()
    {
        _hashids = new Hashids();
    }

    public string GetCopilotId(Guid id)
    {
        var guidChars = id.ToString().ToCharArray();
        var numbers = guidChars.Select(Convert.ToInt32);
        var hash = _hashids.Encode(numbers);
        return hash;
    }

    public Guid? GetEntityId(string copilotId)
    {
        var numbers = _hashids.Decode(copilotId);
        var guidChars = numbers.Select(Convert.ToChar);
        var guidString = string.Join("", guidChars);
        var isGuid = Guid.TryParse(guidString, out var guid);
        return isGuid ? guid : null;
    }
}
