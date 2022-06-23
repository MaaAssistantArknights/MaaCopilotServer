// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Application.Common.Models;

/// <summary>
///     The model of the result of an action.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public class MaaActionResult<T>
{
    /// <summary>
    ///     The API response.
    /// </summary>
    public readonly MaaApiResponse _maaApiResponse;

    /// <summary>
    ///     The constructor of <see cref="MaaActionResult{T}" />.
    /// </summary>
    /// <param name="maaApiResponse">The API response.</param>
    private MaaActionResult(MaaApiResponse maaApiResponse)
    {
        _maaApiResponse = maaApiResponse;
    }

    /// <summary>
    ///     The status code of the response, not to be confused with the HTTP status code.
    /// </summary>
    public int RealStatusCode => _maaApiResponse.StatusCode;

    /// <summary>
    ///     The data of the response.
    /// </summary>
    public T? RealData => (T?)_maaApiResponse.Data;

    /// <summary>
    ///     The implicit operator from <see cref="MaaActionResult{T}" /> to <see cref="OkObjectResult" />.
    /// </summary>
    /// <param name="result">The <see cref="MaaActionResult{T}" />.</param>
    public static implicit operator OkObjectResult(MaaActionResult<T> result)
    {
        return new OkObjectResult(result._maaApiResponse);
    }

    /// <summary>
    ///     The implicit operator from <see cref="MaaApiResponse" /> to <see cref="MaaActionResult{T}" />.
    /// </summary>
    /// <param name="response">The <see cref="MaaApiResponse" />.</param>
    public static implicit operator MaaActionResult<T>(MaaApiResponse response)
    {
        return new MaaActionResult<T>(response);
    }
}
