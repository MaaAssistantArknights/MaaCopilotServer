// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Action = MaaCopilotServer.Application.Common.Operation.Model.Action;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// Tests <see cref="CreateCopilotOperationCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CreateCopilotOperationCommandTest
{
    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotOperationService _copilotOperationService =
        new CopilotOperationService(Options.Create(new CopilotOperationOption()), new DomainString());

    private static Operation OperationFull => new()
    {
        StageName = "test_stage_name",
        MinimumRequired = "v4.0.0",
        Doc = new Doc
        {
            Title = "test_title",
            TitleColor = "test_title_color",
            Details = "test_details",
            DetailsColor = "test_details_color",
        },
        Operators = new Operator[]
        {
            new()
            {
                Name = "test_oper_0_name",
                Skill = 1
            },
            new()
            {
                Name = "test_oper_1_name",
                Skill = 2
            },
            new()
            {
                Name = "test_oper_2_name"
            }
        },
        Groups = new Group[]
        {
            new()
            {
                Name = "test_group_0_name",
                Operators = new Operator[]
                {
                    new()
                    {
                        Name = "test_g_oper_0_name",
                        Skill = 1
                    },
                    new()
                    {
                        Name = "test_g_oper_1_name",
                        Skill = 2
                    },
                    new()
                    {
                        Name = "test_g_oper_2_name"
                    }
                }
            }
        },
        Actions = new Action[]
        {
            new() { Name = "test_oper_0_name", Location = new []{ 1, 2 }, Direction = "Left" },
            new() { Type = "Skill", Name = "test_oper_0_name" },
            new() { Type = "Retreat", Name = "test_oper_0_name" },
            new() { Type = "Retreat", Location = new[] { 1, 2 } },
            new() { Type = "SkillUsage", SkillUsage = 1 },
            new() { Name = "test_oper_0_name", Location = new[] { 1, 2 }, Direction = "Left" },
            new() { Type = "技能", Name = "test_oper_0_name" },
            new() { Type = "撤退", Name = "test_oper_0_name" },
            new() { Type = "撤退", Location = new[] { 1, 2 } },
            new() { Type = "技能用法", SkillUsage = 1 }
        }
    };

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var testContent = JsonSerializer.Serialize(OperationFull,
            new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

        var test = new HandlerTest();
        test.OperationProcessService.SetupValidate(testContent, new()
        {
            IsValid = true,
            Operation = OperationFull,
            ArkLevel = new(new("test_stage_name")),
        });
        test.CurrentUserService.SetupGetUser(new(string.Empty, string.Empty, string.Empty, UserRole.User, Guid.Empty));
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var result = test.TestCreateCopilotOperation(new()
        {
            Content = testContent,
        });

        var id = ((CreateCopilotOperationDto)result.Response.Data!).Id;
        result.DbContext.CopilotOperations.Any().Should().BeTrue();
        var entity = result.DbContext.CopilotOperations.FirstOrDefault();
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(_copilotOperationService.DecodeId(id));
        entity.Content.Should().Be(testContent);
        entity.ArkLevel.LevelId.Should().Be(OperationFull.StageName);
        entity.MinimumRequired.Should().Be(OperationFull.MinimumRequired);
        entity.Title.Should().Be(OperationFull.Doc?.Title ?? string.Empty);
        entity.Details.Should().Be(OperationFull.Doc?.Details ?? string.Empty);

        var entityOperators = (from @operator in OperationFull.Operators ?? Array.Empty<Operator>()
                               select $"{@operator.Name}::{@operator.Skill?.ToString(CultureInfo.InvariantCulture) ?? "1"}").Distinct().ToList();
        entity.Operators.Should().BeEquivalentTo(entityOperators);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> with invalid operations.
    /// </summary>
    [TestMethod]
    public void TestHandleInvalid()
    {
        var test = new HandlerTest();
        test.OperationProcessService.SetupValidate(null, new()
        {
            IsValid = false,
        });
        var result = test.TestCreateCopilotOperation(new()
        {
            Content = null,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.DbContext.CopilotOperations.Any().Should().BeFalse();
    }
}
