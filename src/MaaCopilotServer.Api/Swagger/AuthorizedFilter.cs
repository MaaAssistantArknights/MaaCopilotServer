// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Security;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MaaCopilotServer.Api.Swagger;

/// <summary>
///     Filter to add the Authorization Requirement to the swagger document.
/// </summary>
public class AuthorizedFilter : IOperationFilter
{
    /// <summary>
    ///     Apply the filter to the operation.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attr = context.MethodInfo
            .GetParameters()
            .FirstOrDefault()?
            .ParameterType
            .GetCustomAttributes(true)
            .OfType<AuthorizedAttribute>()
            .FirstOrDefault();

        if (attr is null)
        {
            return;
        }

        var role = attr.Role.ToString().ToUpper();
        var allowInactivatedStr = attr.AllowInActivated
            ? "ALLOW" : "DO NOT ALLOW";

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        operation.Description += $"This API requires \"{role}\" role and \"{allowInactivatedStr}\" inactivated account to access.";
    }
}
