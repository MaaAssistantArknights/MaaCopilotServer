// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Models;

namespace MaaCopilotServer.Api.Swagger;

/// <summary>
///     The maa server api response model.
/// </summary>
/// <remarks>This model is only used to generate the swagger documentation.</remarks>
/// <typeparam name="T">The response data type.</typeparam>
public record MaaApiResponseModel<T> : MaaApiResponse
{
    /// <summary>
    ///     The response data. "{}" means null. Because swagger could not set nullable type, we use "{}" instead.
    /// </summary>
    public new T? Data { get; set; }
}
