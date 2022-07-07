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

        var failed = false;

        if (operationObj.Actions is null)
        {
            return new OperationValidationResult
            {
                IsValid = true, Operation = operationObj, ErrorMessages = string.Empty, ArkLevel = level
            };
        }

        foreach (var action in operationObj.Actions)
        {
            if (failed)
            {
                return new OperationValidationResult
                {
                    IsValid = false,
                    Operation = null,
                    ErrorMessages = _validationErrorMessage.CopilotOperationJsonIsInvalid!,
                    ArkLevel = null
                };
            }

            var type = GetTypeUnifiedString(action.Type);

            failed = type switch
            {
                // When type is "Deploy", operator name, deploy location and deploy direction could not be null.
                "deploy" => action.Name is null || action.Location is null || DirectionIsValid(action.Direction) is false,
                // When type is "Skill", operator name could not be null.
                "skill" => action.Name is null,
                // When type is "Retreat", operator name and deploy location could not be null at the same time.
                "retreat" => action.Name is null && action.Location is null,
                // When type is "Skill Usage", skill_usage could not be null.
                "speedup" => failed,
                "bullettime" => failed,
                "skillusage" => action.SkillUsage is null,
                "output" => failed,
                "skilldaemon" => failed,
                _ => true
            };
        }

        return new OperationValidationResult
        {
            IsValid = true, Operation = operationObj, ErrorMessages = string.Empty, ArkLevel = level
        };
    }

    private static string GetTypeUnifiedString(string? type) => type?.ToLower() switch
    {
        null or "部署" => "deploy",
        "技能" => "skill",
        "撤退" => "retreat",
        "二倍速" => "speedup",
        "子弹时间" => "bullettime",
        "技能用法" => "skillusage",
        "打印" => "output",
        "摆完挂机" => "skilldaemon",
        _ => type.ToLower()
    };

    private static bool DirectionIsValid(string? direction) => direction?.ToLower() switch
    {
        "Left" or "Right" or "Up" or "Down" or "None" => true,
        "左" or "右" or "上" or "下" or "无" => true,
        _ => false
    };
}
