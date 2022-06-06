// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Application.Common.Models;

public class MaaActionResult<T>
{
    private readonly MaaApiResponse _maaApiResponse;
    public int RealStatusCode => _maaApiResponse.StatusCode;
    private MaaActionResult(MaaApiResponse maaApiResponse)
    {
        _maaApiResponse = maaApiResponse;
    }

    public static implicit operator OkObjectResult(MaaActionResult<T> result)
    {
        return new OkObjectResult(result._maaApiResponse);
    }

    public static implicit operator MaaActionResult<T>(MaaApiResponse response)
    {
        return new MaaActionResult<T>(response);
    }
}
