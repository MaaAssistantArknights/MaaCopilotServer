// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using Json.Schema;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Operation.Model;
using MaaCopilotServer.Domain.Constants;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Infrastructure.Services;

public class OperationProcessService : IOperationProcessService
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ValidationErrorMessage _validationErrorMessage;
    private readonly JsonSchema _schema;

    public OperationProcessService(
        IMaaCopilotDbContext dbContext,
        ValidationErrorMessage validationErrorMessage,
        IOptions<ApplicationOption> applicationOption)
    {
        _dbContext = dbContext;
        _validationErrorMessage = validationErrorMessage;

        var schemaFile = Path.Combine(applicationOption.Value.AssemblyPath, SystemConstants.MaaCopilotSchemaPath);
        _schema = JsonSchema.FromFile(schemaFile);
    }

    public async Task<OperationValidationResult> Validate(string? operation)
    {
        if (operation is null)
        {
            return new OperationValidationResult
            {
                IsValid = false,
                Operation = null,
                ErrorMessages = _validationErrorMessage.CopilotOperationJsonIsInvalid!,
                ArkLevel = null
            };
        }

        var schemaValidationResult = _schema.Validate(operation);
        if (schemaValidationResult.IsValid is false)
        {
            var message = _validationErrorMessage.CopilotOperationJsonIsInvalid! + schemaValidationResult.Message;
            return new OperationValidationResult
            {
                IsValid = false,
                Operation = null,
                ErrorMessages = message,
                ArkLevel = null
            };
        }

        var operationObj = JsonSerializer.Deserialize<Operation>(operation);

        var levelId = operationObj!.StageName;
        var level = await _dbContext.ArkLevelData.FirstOrDefaultAsync(x => x.LevelId == levelId);
        if (level is null)
        {
            return new OperationValidationResult
            {
                IsValid = false,
                Operation = null,
                ErrorMessages = _validationErrorMessage.CopilotOperationUnknownLevel!,
                ArkLevel = null
            };
        }

        return new OperationValidationResult
        {
            IsValid = true,
            Operation = operationObj,
            ErrorMessages = string.Empty,
            ArkLevel = level
        };
    }
}
